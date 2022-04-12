using System;
using System.Collections.Generic;
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
        public static string WrapPath => Path.Join(WrapDir, "FaceFXWrapperExtended.exe");
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
            DeployFaceFX();
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
        private static void DeployFaceFX(string relativeTempPath = "Wrapper")
        {
            Create();
            File.WriteAllBytes(WrapPath, Properties.Resources.FaceFXWrapperExtended);
        }

        private static void DeployFonixData(string relativeTempPath = "Wrapper")
        {
            if (!(DependencyCheck.CheckFonixData())) throw new IOException(@"Fonix data not found");
            Create();
            var appDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(appDirectory, "FonixData.cdf");
            // Move the file from path to the temp directory
            File.Copy(path, Path.Join(FullTempDir, relativeTempPath, @"FonixData.cdf"));
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
