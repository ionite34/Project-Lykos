using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;

namespace Project_Lykos
{
    public class ProcessTask
    {
        // Parent
        private readonly ProcessControl pc;

        // Identity
        public Identity TaskIdentity { get; set; }

        // Defines the ProcessTask object for the queue
        public string TaskName { get; set; } = "";
        public string WavSourcePath { get; set; } = "";
        public string WavOutputPath { get; set; } = "";
        public string WavResamplingCachePath { get; set; }
        public string LipOutputPath { get; set; } = "";
        public bool UseFaceFXAudioSampler { get; set; } = false;

        // Wrapper Components
        public string? ProcessID { get; private set; }

        // Wrapper Command
        public Command? SubProcess { get; private set; }
        public string Arg_Type { get; private set; } = "Skyrim";
        public string Arg_Lang { get; private set; } = "USEnglish";
        public string Arg_FonixDataPath { get; private set; }

        // Timing and termination
        public DateTime StartTime { get; private set; }
        public int TimeLimit { get; private set; } // If time limit is reached, we pass up a TaskCanceledException of Type OperationCanceledException


        // Constructor
        // We pass in the ProcessControl parent to get some directories
        public ProcessTask(ProcessControl parent)
        {
            // Set parent
            pc = parent;

            // Create Identity
            TaskIdentity = new Identity();

            // Set the WaveResamplingCachePath to the parent's WaveResamplingCachePath plus the file format:
            // "<UUID>_ResampledTemp.wav"
            WavResamplingCachePath = Path.Join(pc.AudioResamplingCacheDir, TaskIdentity.UUID, "_ResampledTemp.wav");

            // Set Args
            Arg_FonixDataPath = Path.Join(Directory.GetCurrentDirectory(), "FonixData.cdf");

        }

        // Sets WavSourcePath from FileName only
        public void SetWavSourceFile(string FileName)
        {
            WavSourcePath = Path.Join(pc.AudioDir, FileName);
        }

        // Asynchronous processing
        public async Task ProcessAsync()
        {
            // Create the command
            SubProcess = Cli.Wrap(pc.FaceFXPath);
        }
    }
}
