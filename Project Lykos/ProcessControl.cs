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
        // Stores a queue of tasks
        public Queue<ProcessTask> CurrentTaskBatch = new();

        // Constructor
        public ProcessControl()
        {
        }

        // Initializes temp directories to prepare for processing
        private void SetupTemp()
        {
            Cache.Create();
            Cache.DeployFaceFX();
            Cache.DeployFonixData();
        }
    }
}
