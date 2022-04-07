using System;
using System.Collections.Generic;
using System.Data;
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
        public Queue<ProcessTask> CurrentTaskBatch { get; }
        
        // Constructor
        public ProcessControl()
        {
            CurrentTaskBatch = new Queue<ProcessTask>();
        }

        // Sets task batch from DataTable
        public DataTable GenerateTasks(DataTable dataTable)
        {
            return new DataTable();
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
