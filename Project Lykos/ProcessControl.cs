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
        private readonly LykosController? ct;

        
        // Stores a queue of tasks
        public Queue<ProcessTask> CurrentTaskBatch = new();

        // Constructor
        public ProcessControl(LykosController parent)
        {
            this.ct = parent;
        }
    }
}
