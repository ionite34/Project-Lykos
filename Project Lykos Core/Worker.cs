using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    internal class Worker
    {
        // Stores a queue of tasks
        public Queue<ProcessTask> CurrentTaskBatch { get; }

        // Constructor
        public Worker()
        {
            // Create a new task batch
            CurrentTaskBatch = new Queue<ProcessTask>();
        }
    }
}
