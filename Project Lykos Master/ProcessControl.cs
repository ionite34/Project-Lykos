using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CliWrap;
using NAudio.Wave;

namespace Project_Lykos
{
    public class DbProcessControl : Queue<ProcessTask>
    {
        // Events
        public event Action<int,int> ProgressChanged;
        
        // Settings
        public ResamplingMode ResampleMode { get; set; }
        public int MaxRetryCount { get; set; } = 1; // Max number of retries for a failed task, 0 = no retry
        
        // Batch Indicators
        public int BatchSize { get; set; }
        public int CurrentBatch { get; set; }
        public int TotalBatches { get; set; }
        
        // Inner Batch Indicators
        private int errorCount;
        private int timeOutCount;
        public int processedCount;
        private int lastReportedCount;
        
        // List of workers
        public List<SubProcessing> Workers { get; }= new List<SubProcessing>();
        
        // Resampling Mode
        public enum ResamplingMode
        {
            None = 0,
            Native = 1,
            Standard = 2,
            Filtered = 3
        }

        private void OnProgressChanged(int progressCount)     // Progress Changed Event
        {
            ProgressChanged?.Invoke(progressCount, CurrentBatch);
        }

 
        private void CheckProgress()
        {
            if (lastReportedCount == processedCount) return;
            OnProgressChanged(processedCount);
            lastReportedCount = processedCount;
        }

        public Task EStop()
        {
            return Task.Run(() =>
            {   
                // If any FaceFXWrapper are running, shut them down
                foreach (var worker in Workers)
                {
                    worker.StopAll();
                }
                // Wait until all FaceFXWrapper are closed by checking worker's IsRunning status
                while (Workers.Any(worker => worker.IsRunning()))
                {
                    Task.Delay(100).Wait();
                }
            });
        }
        
        // Run Process
        public async Task Start(int batchSize, bool useNativeResampling, CancellationTokenSource cts)
        {
            Cache.Setup();
            BatchSize = batchSize;
            TotalBatches = (int)Math.Ceiling((double)this.Count / batchSize);
            var options = new ParallelOptions()
            {
                CancellationToken = cts.Token
            };
            while (this.Count > 0)
            {
                CurrentBatch++;
                // Batch size is either the remaining tasks or the max batch size
                var currentBatchSize = Math.Min(batchSize, this.Count);
                // Build the current batch by removing the first batchSize tasks from the queue
                var currentBatch = Enumerable.Range(0, currentBatchSize).Select(i => this.Dequeue()).ToList();
                var batchQueue = new Queue<ProcessTask>(currentBatch); // convert list to queue
                // If not using native resampling, we need to convert audio also
                if (!useNativeResampling)
                {
                    try
                    {
                        // Iterate through the current batch and convert audio
                        Parallel.ForEach(currentBatch, options, processTask =>
                        {
                            var sourcePath = processTask.WavSourcePath;
                            var targetPath = processTask.WavTempPath;
                            AudioProcessing.Resample(sourcePath, targetPath, 16000, 1);
                        });
                    }
                    catch (TaskCanceledException)
                    {
                        throw new Exception("Processing canceled.");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error while converting audio: " + ex.Message);
                    }
                }
                
                // Start the batch
                try
                {
                    using (var db = new Data.LykosQueueContext())
                    {
                        await EnqueueBatch(batchQueue, cts, useNativeResampling, db);
                    }
                }
                finally
                {
                    // Cleanup
                    foreach (var processTask in currentBatch)
                    {
                        // Delete Temp File
                        File.Delete(processTask.WavTempPath);
                    }
                }
            }
        }

        // New Process Batch
        private async Task EnqueueBatch(Queue<ProcessTask> batch, CancellationTokenSource cts, bool useNativeResampling, Data.LykosQueueContext dbContext)
        {
            errorCount = 0;
            timeOutCount = 0;
            processedCount = 0;
            lastReportedCount = 0;

            // Dequeue a task and get the command
            while (batch.TryDequeue(out var processTask))
            {
                // Check if the output lip folder exists, if not create it
                if (!Directory.Exists(Path.GetDirectoryName(processTask.LipOutputPath)))
                {
                    var target = Path.GetDirectoryName(processTask.LipOutputPath)!;
                    Directory.CreateDirectory(target);
                }

                // CS: push the item into the queue
                Data.QueueEntry entry = new()
                {
                    Enqueued = DateTimeOffset.Now,
                    WavSourcePath = processTask.WavSourcePath,
                    WavTempPath = processTask.WavTempPath,
                    LipOutputPath = processTask.LipOutputPath,
                    Text = processTask.Text,
                    UseNativeResampler = processTask.UseNativeResampler,
                    Command = processTask.GetCommand()
                };
                dbContext.QueueEntries.Add(entry);

                // Write time to cache
                // timer.Stop();
                // Cache.ProcessingTimes.Add(timer.ElapsedMilliseconds);
                // Increment processed count
                Interlocked.Increment(ref processedCount);

                if (cts.IsCancellationRequested) break;
            }
            await dbContext.SaveChangesAsync();
        }        
    }   
}
