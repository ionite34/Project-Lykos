using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CliWrap;

namespace Project_Lykos
{
    public class ProcessControl
    {
        public string TempDir { get; private set; }
        public string FullTempDir { get; private set; }
        public string WrapDir { get; private set; }
        private readonly string FaceFXName = "FaceFX_Task.exe";
        public string FaceFXPath { get; private set; }
        public string AudioDir { get; private set; }
        public string AudioResamplingCacheDir { get; private set; }
        
        // Stores a queue of tasks
        public Queue<ProcessTask> CurrentTaskBatch = new();

        // Constructor
        public ProcessControl()
        {
            TempDir = Path.GetTempPath();
            FullTempDir = Path.Join(TempDir, "Lykos_Temp");
            WrapDir = Path.Join(FullTempDir, "Wrapper");
            AudioDir = Path.Join(FullTempDir, "AudioSource");
            FaceFXPath = Path.Join(FullTempDir, WrapDir, "FaceFX", FaceFXName);
            AudioResamplingCacheDir = Path.Join(FullTempDir, "ResampleCache");
        }

        // Deploy all folders and structures before we start the processing
        public void DeployTempFiles()
        {
            CreateTempFolder();
            
            // Extract the exe file from the resource to the temp folder
            File.WriteAllBytes(WrapDir, Properties.Resources.FaceFXWrapper);
            
            // Check that the file is there

        }

        // Clean up and delete the temp folder
        public void CleanupTemp()
        {
            // Safety check
            if (FullTempDir.Contains(Path.GetTempPath()) && FullTempDir.Contains("Lykos_Temp"))
            {
                if (Directory.Exists(FullTempDir))
                {
                    try
                    {
                        Directory.Delete(FullTempDir, true);
                    }
                    catch (Exception ex)
                    {
                        throw new IOException("Failed to cleanup temp directory: " + ex);
                    }
                }
            }

        }

        // If the temp folder does not exist in the application directory,
        // create a hidden temp folder.
        public void CreateTempFolder()
        {
            if (!Directory.Exists(FullTempDir))
            {
                try
                {
                    Directory.CreateDirectory(FullTempDir);
                    // Create a subfolder called 'Wrapper' for the exe file
                    Directory.CreateDirectory(Path.Join(FullTempDir, "Wrapper"));
                    // Create a subfolder called 'Source_Audio' for the converted audio files for the current batch
                    Directory.CreateDirectory(Path.Join(FullTempDir, "Source_Audio"));
                }
                catch (Exception ex)
                {
                    throw new IOException("Failed to create temp directory: " + ex);
                }
            }
        }
    }
}
