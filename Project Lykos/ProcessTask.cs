using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using static Project_Lykos.Cache;

namespace Project_Lykos
{
    public class ProcessTask
    {
        // Essential Parameters
        public string WavSourcePath { get; }
        public string LipOutputPath { get; }
        public string Text { get; }
        public bool UseNativeResampler { get; set; }
        public string WavTempPath { get; }
        
        // Counter for the number of times this task was retried
        public int RetryCount { get; set; }

        // Identity
        private string UUID { get; } = Guid.NewGuid().ToString();

        // Constructors
        public ProcessTask(string sourceFile, string outputFile, string text, bool useNative = false)
        {
            // Set essential parameters
            WavSourcePath = sourceFile;
            LipOutputPath = outputFile;
            // Remove any quotes from text
            Text = text.Replace("\"", "");
            UseNativeResampler = useNative;
            WavTempPath = GetTempWavPath(WavSourcePath);
        }
        
        /// <summary>
        /// Get the temp wav path from the full path
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetTempWavPath(string sourcePath)
        {
            // For using the custom resampler, set path to temp directory
            var subFolder =Path.GetFileName(Path.GetDirectoryName(sourcePath));
            var fileName = Path.GetFileName(sourcePath);
            var relativePath = Path.Join(subFolder, fileName);
            return Path.Join(Cache.AudioDir, relativePath);
        }
        
        /// <summary>
        /// Begins task process
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <param name="timeOut"></param>
        /// <returns>
        /// 0 if success, 1 if error, 2 if timeout
        /// </returns>
        public async Task<int> ProcessAsync(CancellationToken controlToken, TimeSpan timeOut)
        {
            // Check if the target lip file already exists, if so, return
            if (File.Exists(LipOutputPath))
            {
                return 0;
            }
            // Create our own time based cancellation token source
            var timeBasedTokenSource = new CancellationTokenSource();
            timeBasedTokenSource.CancelAfter(timeOut);
            // Create a combined token source
            using var linkedTokenSource = 
                CancellationTokenSource.CreateLinkedTokenSource(timeBasedTokenSource.Token, controlToken);

            // Build argument string
            string[] args;
            if (!UseNativeResampler)
            {
                // If not using native, use the temp path only
                args = new string[]
                {
                    ArgType,
                    ArgLang,
                    DataPath,
                    WavTempPath,
                    LipOutputPath,
                    Text
                };
            }
            else
            {
                // If using native, use direct path, provide temp path as well
                args = new string[]
                {
                    ArgType,
                    ArgLang,
                    DataPath,
                    WavSourcePath,
                    WavTempPath,
                    LipOutputPath,
                    Text
                };
            }
            
            // If the folder of the output path doesn't exist, create it
            if (!Directory.Exists(Path.GetDirectoryName(LipOutputPath)))
            {
                var outputDir = Path.GetDirectoryName(LipOutputPath);
                if (outputDir == null)
                {
                    throw new Exception("Output path is null");
                }
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Add the running process to Cache as record
                Cache.ProcessesRunning++;
                var cmd = Cli.Wrap(WrapPath).WithArguments(args).WithValidation(CommandResultValidation.None);
                var task = await cmd.ExecuteBufferedAsync(linkedTokenSource.Token);
                if (task.ExitCode != 0)
                {
                    // In case of CLI error, write the std output and std error to a log file
                    if (!Directory.Exists(LogDir)) Directory.CreateDirectory(LogDir);
                    var logPath = Path.Join(LogDir, UUID + ".log");
                    var log = new StringBuilder();
                    log.AppendLine("==========================================================");
                    log.AppendLine("Resampling mode: " + (UseNativeResampler ? "Native" : "Custom"));
                    log.AppendLine("Path to Audio: " + WavSourcePath);
                    log.AppendLine("Path to Resampled Audio: " + WavTempPath);
                    log.AppendLine("Path to Output: " + LipOutputPath);
                    log.AppendLine("Audio File Name: " + Path.GetFileName(WavSourcePath));
                    log.AppendLine("Text: " + Text);
                    log.AppendLine("==========================================================");
                    log.AppendLine("Exit Code: " + task.ExitCode);
                    log.AppendLine("Run Time: " + task.RunTime);
                    log.AppendLine("==========================================================");
                    log.AppendLine("FaceFX output:");
                    log.AppendLine(task.StandardOutput);
                    log.AppendLine("==========================================================");
                    log.AppendLine("FaceFX error:");
                    log.AppendLine(task.StandardError);
                    log.AppendLine("==========================================================");
                    await File.WriteAllTextAsync(logPath, log.ToString());
                    return 1;
                }
                else
                {
                    // For successful runs, write the run time to cache
                    var runTime = task.RunTime.Milliseconds;
                    Cache.ProcessingTimes.Add(runTime);
                    return 0;
                }
            }
            catch (TaskCanceledException e)
            {
                return 2; // Return 2 for timeout
            }
            finally
            {
                Cache.ProcessesRunning--;
            }
        }
        
        /// <summary>
        /// Returns the current task as a command string
        /// </summary>
        /// <returns>
        /// Command string with quotes
        /// </returns>
        public string GetCommand()
        {
            string[] cmdArgs;
            if (!UseNativeResampler)
            {
                // If not using native, use the temp path only
                cmdArgs = new string[]
                {
                    WavTempPath,
                    LipOutputPath,
                    Text
                };
            }
            else
            {
                // If using native, use direct path, provide temp path as well
                cmdArgs = new string[]
                {
                    WavSourcePath,
                    WavTempPath,
                    LipOutputPath,
                    Text
                };
            }
            // Build string by surrounding each element in quotes
            var cmdString = new StringBuilder();
            foreach (var arg in cmdArgs)
            {
                // cmdString.Append($"\"{arg}\"|"); // Use | as delimiter
                cmdString.Append($"{arg}|");
            }
            var output = $"\"{cmdString}\""; // Surround with quotes
            return output;
        }
    }
}
