using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Project_Lykos;

public class SubProcessingAdv
{
    // Configs
    private readonly TimeSpan startTimeout = TimeSpan.FromSeconds(15);
    private readonly TimeSpan processTimeout = TimeSpan.FromSeconds(20);

    // Process info
    private readonly CancellationTokenSource cancelControl = new();
    private readonly string uuid = Guid.NewGuid().ToString();
    private readonly int processNumber;
    
    // Process
    private System.Diagnostics.Process? fxProcess;
    private bool isRunning;

    // Arguments
    private readonly string dataPath;
    private readonly string procPath = Cache.WrapPath;
    private readonly string argType = Cache.ArgType;
    private readonly string argLang = Cache.ArgLang;
    private readonly bool useNativeResampler;
    
    public SubProcessingAdv(int processNumber, bool useNativeResampler = false)
    {
        this.processNumber = processNumber;
        this.useNativeResampler = useNativeResampler;
        dataPath = Cache.DataPath;
        // Deploy the FonixData for the subprocess
        // var dataName = "FXData_" + uuid + ".cdf";
        // dataPath = Path.Join(Cache.WrapDir, dataName);
        // Cache.DeployFonixData(dataName);
    }
    
    /// <summary>
    /// Returns true if the process is running
    /// </summary>
    /// <returns></returns>
    public bool IsRunning()
    {
        if (fxProcess == null) return false;

        try
        {
            Process.GetProcessById(fxProcess.Id);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return !fxProcess.HasExited;
    }

    /// <summary>
    /// Stops the process and cancels ongoing operations
    /// </summary>
    public void StopAll()
    {
        if (fxProcess == null) return;
        cancelControl.Cancel();
        fxProcess.Kill();
        fxProcess.WaitForExit();
        isRunning = false;
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
            return 1; // Successful start
        }
        if (outputLine.Contains("Error"))
        {
            return 2; // Error
        }
        return 0; // No parsable event
    }

    /// <summary>
    /// Starts the subprocess and listens to stdIn
    /// </summary>
    public int StartSubProcess()
    {
        try
        {
            // Create process and info
            fxProcess = new Process();
            var startInfo = new ProcessStartInfo()
            {
                FileName = procPath,
                ArgumentList = { @"fxe", argType, argLang, dataPath, useNativeResampler ? "true" : "false" },
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            fxProcess.StartInfo = startInfo;
            
            // Start process
            fxProcess.Start();

            // Set affinity, only if processNumber is not 0
            if (processNumber > 0)
            {
                var affinity = (1 << processNumber);
                var affinityMask = new IntPtr(affinity);
                fxProcess.ProcessorAffinity = affinityMask;
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error starting subprocess", e);
        }
        
        // Create our own time based cancellation token
        var timeoutToken = new CancellationTokenSource(startTimeout);

        // Listen to stdOut
        var taskResult = 0;
        while (!cancelControl.IsCancellationRequested && !timeoutToken.IsCancellationRequested)
        {
            var output = fxProcess.StandardOutput.ReadLine();
            if (output == null) continue;
            var parsedResult = ParseStartup(output);
            if (parsedResult != 1 && parsedResult != 2) continue;
            taskResult = parsedResult;
            break;
        }
        // Shutdown if error or timeout
        if (taskResult is 2 or 0)
        {
            fxProcess.Kill();
        }
        // Set flag
        isRunning = taskResult is 1;
        // Return result
        return taskResult;
    }
    
    public int DoTask(string command)
    {
        // Check process was started
        if (fxProcess == null || !isRunning)
        {
            var result = StartSubProcess();
            if (result is 2 or 0)
            {
                throw new Exception("Subprocess failed to start");
            }
        }
        
        // Write command to stdIn
        fxProcess!.StandardInput.WriteLine(command);
        fxProcess!.StandardInput.Flush();

        // Time based cancellation token
        var timeoutToken = new CancellationTokenSource(processTimeout);

        // Create a combined cancellation token
        var combinedToken = CancellationTokenSource.CreateLinkedTokenSource(cancelControl.Token, timeoutToken.Token).Token;
        
        // Listen to stdOut
        var taskResult = 0;
        var output = new List<string>(); // List of lines of output
        while (!combinedToken.IsCancellationRequested)
        {
            var line = fxProcess.StandardOutput.ReadToEnd(); // Read line
            if (line == null) break; // Return timeout error if no output
            output.Add(line); // Add to output list
            var parsedResult = ParseOutput(line); // Parse output
            if (parsedResult is not (1 or 2)) continue; // Skip if not a valid result
            taskResult = parsedResult; // If valid result, set task result
            break; // Break loop
        }
        // Shutdown if error or timeout
        if (taskResult is 2 or 0)
        {
            Task.Run(() => WriteLog(output));
            fxProcess.Kill();
            isRunning = false;
        }
        // Return result
        return taskResult;
    }


    /// <summary>
    /// Writes a string builder to a log file
    /// </summary>
    /// <param name="stringBuilder"></param>
    private static async Task WriteLog(StringBuilder stringBuilder)
    {
        Directory.CreateDirectory(Cache.LogDir);
        var uuid = Guid.NewGuid().ToString();
        var fileName = Path.Combine(Cache.LogDir, $"{uuid}_FaceFX_Error.log");
        await using var writer = File.CreateText(fileName);
        await File.WriteAllTextAsync(fileName, stringBuilder.ToString());
        await writer.FlushAsync();
    }
    
    /// <summary>
    /// Writes a string builder to a log file (String list)
    /// </summary>
    /// <param name="stringList"></param>
    private static async Task WriteLog(List<string> stringList)
    {
        Directory.CreateDirectory(Cache.LogDir);
        var uuid = Guid.NewGuid().ToString();
        var fileName = Path.Combine(Cache.LogDir, $"{uuid}_FaceFX_Error.log");
        await using var writer = File.CreateText(fileName);
        await File.WriteAllTextAsync(fileName, stringList.ToString());
        await writer.FlushAsync();
    }
}