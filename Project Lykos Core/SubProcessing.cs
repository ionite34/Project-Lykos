using System.Diagnostics;
using System.Reflection;
using System.Text;
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

    // Private vars
    private readonly StringBuilder stdOutBuffer = new StringBuilder();
    private readonly MemoryStream stream = new();
    private readonly StreamWriter stdInWriter;
    private readonly CancellationTokenSource cancelControl = new();
    private readonly string uuid = Guid.NewGuid().ToString();
    private readonly string dataPath;
    private readonly bool writeLogsToFile = false;
    public readonly int processNumber;
    private System.Diagnostics.Stopwatch _startProcessTimer = null;
    private System.Diagnostics.Stopwatch _doWorkTimer = null;

    // Private objects
    private SubProcess? subProcess;
    
    public SubProcessing(int processNumber, bool writeLogsToFiles)
    {
        this.processNumber = processNumber;
        this.writeLogsToFile = writeLogsToFiles;
        stdInWriter = new StreamWriter(stream);
        // Deploy the FonixData for the subprocess
        var dataName = "FXData_" + uuid + ".cdf";
        dataPath = Path.Join(Cache.WrapDir, dataName);
        Cache.DeployFonixData(dataName);
    }

    public bool IsRunning()
    {
        return subProcess is {IsAlive: true};
    }

    /// <summary>
    /// Diagnostic access to the time it took to start the process
    /// </summary>
    public TimeSpan? StartupTime { get => _startProcessTimer?.Elapsed; }
    /// <summary>
    /// Diagnostic access to the time it took to execute the last work command
    /// </summary>
    public TimeSpan? DoWorkTime { get => _doWorkTimer?.Elapsed; }

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
        if (outputLine.Contains("[FXE] > RFI"))
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
            subProcess = new SubProcess( WrapPath, "fxe", ArgType, ArgLang, 
                dataPath, UseNativeResampler.ToString().ToLower())
            {
                Out = SubProcess.Pipe,
                In = SubProcess.Pipe
            };
            subProcess.Start();
            if (!subProcess.HasStarted) return false;

            // CS: only assign processor affinity if process Number is greater than (or equal to) 0 and less than the number of available cores;
            if (processNumber >= 0 && processNumber < Environment.ProcessorCount)
            {
                var affinity = (1 << processNumber);
                var processMember = subProcess.GetType().GetField(("Process"), BindingFlags.Instance | BindingFlags.NonPublic);
                var processValue = (System.Diagnostics.Process)processMember.GetValue(subProcess);
                processValue.ProcessorAffinity = (IntPtr)affinity;
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error starting subprocess", e);
        }
        // Clear stdout buffer
        stdOutBuffer.Clear();
        // Create our own time based cancellation token
        var timeoutToken = new CancellationTokenSource(StartTimeout);
        // Read from the out
        var reader = subProcess.OutputReader;
        var readCount = 0;
        _startProcessTimer = System.Diagnostics.Stopwatch.StartNew();
        var readTimer = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            // Continuously read from the stream for updates
            while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
            {
                // Skip if less than 25ms since last read and not first run
                if ((readCount != 0) && (readTimer.ElapsedMilliseconds < 25)) continue;

                readCount++;
                readTimer.Reset();

                // Read from the stream
                var parseResult = 0;
                // This auto continues if no more lines
                while (reader.ReadLine() is { } outputLine)
                {
                    if (cancelControl.IsCancellationRequested) break;
                    if (String.IsNullOrEmpty(outputLine)) continue;
                    parseResult = ParseStartup(outputLine);
                    switch (parseResult)
                    {
                        case 1:
                            return true;
                        case 2:
                            // If error
                            _startProcessTimer.Stop();
                            subProcess.Kill();
                            Task.Run(() => WriteLog());
                            return false;
                    }
                }
            }
        }
        finally
        {
            _startProcessTimer.Stop();
        }

        // For timeout
        subProcess.Kill();
        Task.Run(() => WriteLog());
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
        
        // Time based cancellation token
        var timeoutToken = new CancellationTokenSource(ProcessTimeout);
        // Reader for the output stream
        var reader = subProcess.OutputReader;
        // Tracking variables
        var readCount = 0;
        _doWorkTimer = Stopwatch.StartNew();
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        try
        {
            // Start read while not cancelled
            while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
            {
                // Skip if less than 25ms since last read and not first run
                if ((readCount != 0) && (sw.ElapsedMilliseconds < 25)) continue;
                readCount++;
                sw.Restart();
                // This auto continues if no more lines
                var parseResult = 0;
                while (reader.ReadLine() is { } outputLine)
                {
                    if (cancelControl.IsCancellationRequested) break;
                    if (String.IsNullOrEmpty(outputLine)) continue;
                    stdOutBuffer.AppendLine(outputLine);
                    parseResult = ParseOutput(outputLine);
                    switch (parseResult)
                    {
                        case 1:
                            return true;
                        case 2:
                            Task.Run(() => WriteLog());
                            return false;
                    }
                }
            }
        }
        finally
        {
            _doWorkTimer.Stop();
        }

        Task.Run(() => WriteLog());
        subProcess.Kill();
        return false;
    }

    public string FetchAndClearOutputBuffer()
    {
        string output = stdOutBuffer.ToString();
        stdOutBuffer.Clear();
        return output;
    }

    /// <summary>
    /// Writes the current <see cref="stdOutBuffer"/> to a log file, if <seealso cref="writeLogsToFile"/> is enabled.
    /// </summary>
    /// <param name="stringBuilder"></param>
    private async Task WriteLog()
    {
        if (writeLogsToFile)
        {
            Directory.CreateDirectory(LogDir);
            var uuid = Guid.NewGuid().ToString();
            var fileName = Path.Combine(LogDir, $"{uuid}_FaceFX_Error.log");
            using var writer = File.CreateText(fileName);
            await File.WriteAllTextAsync(fileName, FetchAndClearOutputBuffer());
            await writer.FlushAsync();
        }
    }
}