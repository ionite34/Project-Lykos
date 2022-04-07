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
        // Identity
        public Identity TaskIdentity { get; set; }

        // Defines the ProcessTask object for the queue
        public string WavSourcePath { get; set; } = "";
        public string WavResamplingCachePath { get; set; }
        public string LipOutputPath { get; set; } = "";
        public bool UseFaceFXAudioSampler { get; set; } = false;

        // Wrapper Components
        public string? ProcessID { get; private set; }

        // Wrapper Command
        public Command? SubProcess { get; private set; }
        public string Arg_Type { get; private set; } = "Skyrim";
        public string Arg_Lang { get; private set; } = "USEnglish";

        // Timing and termination
        public DateTime StartTime { get; private set; }
        public int TimeLimit { get; private set; } // If time limit is reached, we pass up a TaskCanceledException of Type OperationCanceledException


        // Constructor
        public ProcessTask()
        {
            // Create Identity
            TaskIdentity = new Identity();

            // Set the WaveResamplingCachePath to the parent's WaveResamplingCachePath plus the file format:
            // "<UUID>_ResampledTemp.wav"
            WavResamplingCachePath = Path.Join(Cache.AudioResamplingDir, TaskIdentity.UUID, "_ResampledTemp.wav");
        }

        // Sets WavSourcePath from FileName only
        public void SetWavSourceFile(string FileName)
        {
            WavSourcePath = Path.Join(Cache.AudioDir, FileName);
        }
        
        // Asynchronous processing
        public async Task ProcessAsync()
        {
            // Create the command
            SubProcess = Cli.Wrap(Cache.WrapPath);
        }
    }
}
