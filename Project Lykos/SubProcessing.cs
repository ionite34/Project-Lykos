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
    private readonly ProcessControl processControl;
    private readonly StringBuilder stdOutBuffer = new StringBuilder();
    private readonly MemoryStream stream = new();
    private readonly StreamWriter stdInWriter;
    private readonly CancellationTokenSource cancelControl = new();
    private readonly string uuid = Guid.NewGuid().ToString();
    private readonly string exeName;
    private readonly string exePath;
    private readonly string dataName;
    private readonly string dataPath;
    
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
        // Clear stdout buffer
        stdOutBuffer.Clear();
        // Create our own time based cancellation token
        var timeoutToken = new CancellationTokenSource(StartTimeout);
        // Read from the out
        var reader = subProcess.OutputReader;
        var readCount = 0;
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        // Continuously read from the stream for updates
        while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
        {
            // Skip if less than 25ms since last read and not first run
            if ((readCount != 0) && (sw.ElapsedMilliseconds < 25)) continue;
            readCount++;
            // Read from the stream
            var parseResult = 0;
            // This auto continues if no more lines
            while (reader.ReadLine() is { } outputLine)
            {
                stdOutBuffer.AppendLine(outputLine);
                parseResult = ParseStartup(outputLine);
            }
            switch (parseResult)
            {
                case 1:
                    return true;
                case 2:
                    // If error continue to timeout tasks
                    break;
            }
        }
        // For timeout
        subProcess.Kill();
        WriteLog(stdOutBuffer);
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
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        // Start read while not cancelled
        while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
        {
            // Skip if less than 25ms since last read and not first run
            if ((readCount != 0) && (sw.ElapsedMilliseconds < 25)) continue;
            readCount++;
            // This auto continues if no more lines
            var parseResult = 0;
            while (reader.ReadLine() is { } outputLine)
            {
                stdOutBuffer.AppendLine(outputLine);
                parseResult = ParseOutput(outputLine);
            }
            switch (parseResult)
            {
                case 1:
                    return true;
                case 2:
                    WriteLog(stdOutBuffer);
                    return false;
            }
        }
        sw.Stop();
        WriteLog(stdOutBuffer);
        subProcess.Kill();
        return false;
    }

    /// <summary>
    /// Writes a string builder to a log file
    /// </summary>
    /// <param name="stringBuilder"></param>
    private static void WriteLog(StringBuilder stringBuilder)
    {
        var uuid = Guid.NewGuid().ToString();
        var fileName = Path.Combine(LogDir, $"{uuid}_FaceFX_Error.log");
        using var writer = File.CreateText(fileName);
        File.WriteAllTextAsync(fileName, stringBuilder.ToString());
        writer.FlushAsync();
    }
}