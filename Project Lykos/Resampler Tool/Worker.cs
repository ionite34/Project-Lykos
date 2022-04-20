using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;

namespace Project_Lykos.Resampler_Tool
{
    public class Worker
    {
        // Progress Reporting
        public event Action<int>? ProgressChanged;
        private int lastReportedCount;

        // Counts
        public int Total;
        public int Current;
        public int Failed;

        // Data
        public DataTable? FilesTable;
        private string? outputPath;
        private readonly Resampler parent;
        
        // Settings
        public int SampleRate { get; set; }
        public int Channels { get; set; }

        public Worker(Resampler parent)
        {
            this.parent = parent;
        }

        private void CheckProgress()
        {
            var percent = 100 * (Current - lastReportedCount) / (double) Total;
            if (percent < 0.5) return;
            ProgressChanged?.Invoke(Current);
            lastReportedCount = Current;
        }

        // Start processing
        public async Task Start(int threads, CancellationTokenSource cts)
        {
            // Convert column 1 of the filesTable to a string list
            // List<string> filesList = (from DataRow row in FilesTable.Rows select row[1].ToString()).ToList()!;
            FilesTable = parent.Indexer.IndexData;
            outputPath = parent.OutputPath.Path;
            Total = FilesTable.Rows.Count;
            Current = 0;
            try
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = threads,
                    CancellationToken = cts.Token
                };
                await Task.Run(() =>
                {
                    Parallel.ForEach(FilesTable.AsEnumerable(), options, row =>
                    {
                        var sourcePath = row[1].ToString();
                        if (sourcePath == null)
                        {
                                
                        }

                        var targetPath = Path.Join(outputPath, row[0].ToString());
                        AudioProcessing.Resample(sourcePath, targetPath, SampleRate, Channels);
                        Interlocked.Increment(ref Current);
                        CheckProgress();
                    });
                });
            }
            finally
            {
                // Run cleanup here
            }
        }
    }
}
