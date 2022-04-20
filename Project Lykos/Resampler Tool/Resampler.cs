using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Resampler_Tool
{
    public class Resampler
    {
        // Paths
        public DynamicPath SourcePath { get; set; } = new();
        public DynamicPath OutputPath { get; set; } = new();

        // Indexer
        public readonly IndexControl Indexer = new("*.wav");

        // Worker
        public Worker ProcessWorker;
        
        // Constructor
        public Resampler()
        {
            ProcessWorker = new Worker(this);
        }

        // Returns true if ready to start batch
        public bool ReadyStart()
        {
            return SourcePath.Exists() && OutputPath.Exists();
        }

        public void SetFilepath_Source(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("Directory not found.");
            SourcePath.SetPath(filepath);
            Task.Run(() =>
            {
                Indexer.BackgroundIndexFiles(filepath);
                ProcessWorker.FilesTable = Indexer.IndexData;
            });
        }

        public void SetFilepath_Output(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("Directory not found.");
            OutputPath.SetPath(filepath);
        }
    }
}
