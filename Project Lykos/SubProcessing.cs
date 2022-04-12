using System.Text;
using System.Windows.Forms.VisualStyles;
using CliWrap;
using CliWrap.EventStream;
using Gt.SubProcess;
using static Project_Lykos.Cache;

namespace Project_Lykos;

public class SubProcessing
{
    // Configs
    public bool AutoRestart { set; get; } = false;
    public bool UseNativeResampler { get; set; } = false;
    public TimeSpan StartTimeout { get; set; } = TimeSpan.FromSeconds(15);
    public TimeSpan ProcessTimeout { get; set; } = TimeSpan.FromSeconds(60);
    
    // Info
    public ProcessTask? CurrentTask { private set; get; }
    
    // Private vars
    private readonly ProcessControl processControl;
    private StringBuilder stdOutBuffer = new StringBuilder();
    private readonly MemoryStream stream = new();
    private readonly StreamWriter stdInWriter;
    private CancellationTokenSource cancelControl = new();
    private string uuid = Guid.NewGuid().ToString();
    private string exeName;
    private string exePath;
    private string dataName;
    private string dataPath;
    
    // Private objects
    private SubProcess? subProcess;
    
    public SubProcessing(ProcessControl parent)
    {
        processControl = parent;
        stdInWriter = new StreamWriter(stream);
        exeName = "FXExtended_" + uuid + ".exe";
        exePath = Path.Join(Cache.WrapDir, exeName);
        dataName = "FXData_" + uuid + ".cdf";
        dataPath = Path.Join(Cache.WrapDir, dataName);
        Cache.DeployExecutable(exeName);
        Cache.DeployFonixData(dataName);
    }

    public bool IsRunning()
    {
        return subProcess is {IsAlive: true};
    }
    
    
    /// <summary>
    /// Writes to the process's stdin
    /// </summary>
    /// <param name="text"></param>
    private void WriteToStdIn(string text)
    {
        stdInWriter.Write(text);
        stdInWriter.Flush();
    }

    /// <summary>
    /// Dequeues a task and builds the command string
    /// </summary>
    /// <returns>
    /// Command string with quotes
    /// </returns>
    private string GetNewCommand()
    {
        // Dequeue a processTask from the queue (ProcessControl)
        var processTask = processControl.Dequeue();
        return processTask.GetCommand();
    }

    /// <summary>
    /// This parses the stdout of the program and returns the state
    /// </summary>
    /// <param name="outputLine"></param>
    /// <returns>
    /// 0 - No parsable event, 1 - Successful output, 2 - Error
    /// </returns>
    private static int ParseOutput(string outputLine)
    {
        if (outputLine.Contains("[FXE] > COMPLETED"))
        {
            return 1;
        }
        if (outputLine.Contains("[FXE] > FAILED LIPGEN"))
        {
            return 2;
        }
        return 0;
    }

    /// <summary>
    /// This parses the stdout of the program at startup and returns the state
    /// </summary>
    /// <param name="outputLine"></param>
    /// <returns>
    /// 0 - No parsable event, 1 - Successful start, 2 - Error
    /// </returns>
    private static int ParseStartup(string outputLine)
    {
        if (outputLine.StartsWith("[FXE] > RFI"))
        {
            return 1;
        }
        if (outputLine.Contains("Error"))
        {
            return 2;
        }
        return 0;
    }

    public void StopAll()
    {
        cancelControl.Cancel();
        subProcess?.Kill();
    }

    /// <summary>
    /// Starts the subprocess and listens to stdIn
    /// </summary>
    public bool StartSubProcess()
    {
        try
        {
            subProcess = new SubProcess(exePath, "fxe", ArgType, ArgLang, 
                dataPath, UseNativeResampler.ToString().ToLower())
            {
                Out = SubProcess.Capture,
                In = SubProcess.Pipe
            };
            subProcess.Start();
            if (!subProcess.HasStarted) return false;
        }
        catch (Exception e)
        {
            throw new Exception("Error starting subprocess", e);
        }
        // Read from the out
        var reader = subProcess.OutputReader;
        // Create our own time based cancellation token
        var timeoutToken = new CancellationTokenSource(StartTimeout);
        // Continuously read from the stream for updates
        while (subProcess.IsAlive && !cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
        {
            // Read from the stream
            var outputLine = reader.ReadLine();
            // If there is a line, parse it
            if (outputLine == null) continue;
            if (ParseStartup(outputLine) == 1)
            {
                return true;
            }
            if (ParseStartup(outputLine) == 2)
            {
                // If error, stop
                subProcess.Kill();
                throw new Exception("FaceFX subprocess reported error on startup: " + outputLine);
            }
            // Wait 5ms
            Thread.Sleep(5);
        }
        // For timeout
        subProcess.Kill();
        WriteLog(stdOutBuffer).ConfigureAwait(false);
        return false;
    }

    public bool DoTask(string command)
    {
        // Check process was started
        if (subProcess is not {IsAlive: true})
        {
            throw new Exception("Subprocess not running");
        }
        
        stdOutBuffer.Clear(); // Clear the buffer
        subProcess.WriteLine(command); // Write the command to the input stream
        
        // Create our own time based cancellation token
        var timeoutToken = new CancellationTokenSource(ProcessTimeout);
        // Check output
        var reader = subProcess.OutputReader;
        var readCount = 0;
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
        {
            // Skip if less than 25ms since last read
            if ((readCount != 0) && (sw.ElapsedMilliseconds < 25)) continue;
            readCount++;
            var parseResult = 0;
            // This auto continues if no more lines
            while (reader.ReadLine() is { } outputLine)
            {
                parseResult = ParseOutput(outputLine);
            }
            if (parseResult == 1) return true;
            if (parseResult == 2) return false;
        }
        sw.Stop();
        subProcess.Kill();
        // WriteLog(stdOutBuffer).ConfigureAwait(false);
        return false;
    }

    private void ErrorExit(string message)
    {
        // Kill the subprocess
        subProcess?.Kill();
        Console.WriteLine(message);
        Environment.Exit(1);
    }
    
    /// <summary>
    /// Writes a string builder to a log file
    /// </summary>
    /// <param name="stringBuilder"></param>
    private static async Task WriteLog(StringBuilder stringBuilder)
    {
        var uuid = Guid.NewGuid().ToString();
        var fileName = Path.Combine(LogDir, $"{uuid}_FaceFX_Error.log");
        await using var writer = File.CreateText(fileName);
        await File.WriteAllTextAsync(fileName, stringBuilder.ToString());
        await writer.FlushAsync();
    }
    
    /// <summary>
    /// (Deprecated) Starts the subprocess and listens to stdIn
    /// </summary>
    private async void StartSubProcessLegacy(CancellationTokenSource cts)
    {
        var cmd = (PipeSource.FromStream(stream) | Cli.Wrap(WrapPath)).WithArguments("start");
        await foreach (var cmdEvent in cmd.ListenAsync().WithCancellation(cts.Token))
        {
            switch (cmdEvent)
            {
                case StartedCommandEvent started:
                    stdOutBuffer.AppendLine($"Process started; ID: {started.ProcessId}");
                    break;
                case StandardOutputCommandEvent stdOut:
                    stdOutBuffer.AppendLine($"{stdOut.Text}");
                    break;
                case StandardErrorCommandEvent stdErr:
                    // This is not used by the subprocess
                    break;
                case ExitedCommandEvent exited:
                    stdOutBuffer.AppendLine($"Process exited; Code: {exited.ExitCode}");
                    // Send to ProcessExited
                    break;
            }
        }
    }
}