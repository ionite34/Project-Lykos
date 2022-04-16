using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    // Manages static operations relating to building and destroying the temp directory
    public static class Cache
    {
        // The path to the temp directory
        public static readonly string TempDir = Path.GetTempPath();
        public static string FullTempDir => Path.Join(TempDir, "Lykos_Temp");
        public static string WrapDir => Path.Join(FullTempDir, "Wrapper");
        public static string DataPath => Path.Join(FullTempDir, "Wrapper", "FonixData.cdf");
        public static string WrapPath => Path.Join(WrapDir, "FXExtended.exe");
        public static string AudioDir => Path.Join(FullTempDir, "AudioSource");
        public static string LogDir => Path.Join(Directory.GetCurrentDirectory(), "Logs");

        // Storage of args
        public static string ArgType { get; } = "Skyrim";
        public static string ArgLang { get; } = "USEnglish";
        
        // Storage of processing time
        public static List<double> ProcessingTimes { set; get; } = new List<double>();
        // Returns the average of the processing times
        public static double AverageProcessingTime => ProcessingTimes.Average();
        
        // Running
        public static int ProcessesRunning { set; get; } = 0;
        
        // Setups the temp directory for use
        public static void Setup()
        {
            Destroy();
            Create();
            DeployExecutable();
            DeployFonixData();
        }
        
        // Deletes the temp directory
        public static void Destroy()
        {
            if (!Directory.Exists(FullTempDir)) return;
            Directory.Delete(FullTempDir, true);
        }
        
        // Creates the temp directory
        private static void Create()
        {
            if (Directory.Exists(FullTempDir)) return;
            Directory.CreateDirectory(FullTempDir);
            Directory.CreateDirectory(WrapDir);
            Directory.CreateDirectory(AudioDir);
        }
        
         // Deploys the FaceFXWrapper.exe to the temp directory
        public static void DeployExecutable(string customName = "FXExtended.exe")
        {
            Create();
            var writePath = Path.Join(WrapDir, customName);
            File.WriteAllBytes(writePath, Properties.Resources.FXExtended);
        }

        public static void DeployFonixData(string customName = "FXData.cdf")
        {
            if (!(DependencyCheck.CheckFonixData())) throw new IOException(@"Fonix data not found");
            Create();
            var appDirectory = Directory.GetCurrentDirectory();
            var sourcePath = Path.Combine(appDirectory, "FonixData.cdf");
            // Rename the file to the custom name
            File.Copy(sourcePath, Path.Join(WrapDir, customName));
        }

        public static bool KillProcesses()
        {
            var task = Task.Run(() =>
            {
                // If any FaceFXWrapper are running, shut them down
                foreach (var process in Process.GetProcessesByName("FXExtended"))
                {
                    process.Kill();
                }
                // Start a stopwatch for timeout
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                // Wait until all FaceFXWrapper are closed
                while (Process.GetProcessesByName("FXExtended").Length > 0)
                {
                    if (stopwatch.ElapsedMilliseconds > 5000) return false;
                    Task.Delay(50).Wait();
                }
                try
                {
                    Destroy();
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;
            });
            return task.Result;
        }

        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize) 
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
