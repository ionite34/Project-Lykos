using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Word_Checker
{
    internal class WordParser
    {
        // Paths
        public DynamicPath CSVPath { get; set; } = new();
        public DynamicPath DictPath { get; set; } = new();

        // Returns true if ready to start batch
        public bool ReadyRefresh()
        {
            return CSVPath.Exists() && DictPath.Exists();
        }

        public void SetFilepath_CSV(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("Directory not found.");
            CSVPath.SetPath(filepath);
            Task.Run(() =>
            {
                // Indexer.BackgroundIndexFiles(filepath);
                // ProcessWorker.FilesTable = Indexer.IndexData;
            });
        }

        public void SetFilepath_Dict(string filepath)
        {
            if (!System.IO.Directory.Exists(filepath)) throw new Exception("Directory not found.");
            DictPath.SetPath(filepath);
        }

    }
}
