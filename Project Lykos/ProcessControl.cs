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
    public class ProcessControl : Queue<ProcessTask>
    {
        // Events
        public event Action<int, int> ProgressChanged;

        // Settings
        public ResamplingMode ResampleMode { get; set; }
        public int MaxRetryCount { get; set; } = 1; // Max number of retries for a failed task, 0 = no retry

        // Batch Indicators
        public int BatchSize { get; set; }
        public int CurrentBatch { get; set; }
        public int TotalBatches { get; set; }
        public int TotalProcessed { get; set; }

        // Inner Batch Indicators
        private int errorCount;
        private int timeOutCount;
        public int ProcessedCount;
        private int lastReportedCount;

        // List of workers
        public List<SubProcessing> Workers { get; } = new List<SubProcessing>();

        // Resampling Mode
        public enum ResamplingMode
        {
            None = 0,
            Native = 1,
            Standard = 2,
            Filtered = 3
        }

        private void OnProgressChanged(int progressCount) // Progress Changed Event
        {
            ProgressChanged?.Invoke(progressCount, CurrentBatch);
        }


        private void CheckProgress()
        {
            if (lastReportedCount == ProcessedCount) return;
            OnProgressChanged(ProcessedCount);
            lastReportedCount = ProcessedCount;
        }

        public Task EStop()
        {
            if (Workers.Count == 0) return Task.CompletedTask;
            return Task.Run(() =>
            {
                // If any FaceFXWrapper are running, shut them down
                foreach (var worker in Workers)
                {
                    worker.StopAll();
                }
            });
        }

        // Run Process
        public async Task Start(int processCount, int batchSize, bool useNativeResampling, CancellationTokenSource cts)
        {
            Cache.Setup();
            BatchSize = batchSize;
            TotalBatches = (int) Math.Ceiling((double) this.Count / batchSize);
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = processCount,
                CancellationToken = cts.Token
            };
            while (this.Count > 0)
            {
                CurrentBatch++;

                // Reset counters
                errorCount = 0;
                timeOutCount = 0;
                ProcessedCount = 0;
                lastReportedCount = 0;

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
                    // Start workers if not already running
                    if (!AllWorkersRunning())
                    {
                        await StartWorkers(cts, processCount);
                    }

                    // Do process batch
                    await ProcessBatch(batchQueue, processCount, cts, useNativeResampling);
                }
                finally
                {
                    // Cleanup
                    foreach (var processTask in currentBatch)
                    {
                        // Delete Temp File
                        File.Delete(processTask.WavTempPath);
                    }

                    // Increment the processed count
                    TotalProcessed += ProcessedCount;
                }
            }
        }

        // Returns true if all workers are running
        public bool AllWorkersRunning()
        {
            if (Workers.Count == 0)
            {
                return false;
            }

            return Workers.All(worker => worker.IsRunning());
        }

        // Start all workers
        public async Task StartWorkers(CancellationTokenSource cts, int processCount)
        {
            // Register to stop all subprocesses if the user cancels
            cts.Token.Register(() =>
            {
                foreach (var subProcess in Workers)
                {
                    subProcess.StopAll();
                }
            });

            // Make new workers equal to the process count
            for (var i = 0; i < processCount; i++)
            {
                Workers.Add(new SubProcessing(this, i));
            }

            // Start subprocesses
            var tasks = new List<Task>();
            foreach (var worker in Workers)
            {
                var startTask = Task.Run(() => worker.StartSubProcess());
                tasks.Add(startTask);
            }

            await Task.WhenAll(tasks);

            // Check results
            var results = tasks.Select(task => ((Task<bool>) task).Result).ToList();
            // If any of the subprocesses failed, throw an exception
            if (results.Any(result => !result))
            {
                cts.Cancel();
                throw new Exception("Error while starting subprocesses.");
            }
        }

        // New Process Batch
        private async Task ProcessBatch(Queue<ProcessTask> batch, int processCount, CancellationTokenSource cts,
            bool useNativeResampling)
        {
            var tasks = new List<Task>();
            // Add new tasks
            foreach (var worker in Workers)
                tasks.Add(Task.Run(() => RunWorker(batch, worker, cts)));

            await Task.WhenAll(tasks);
        }

        private void RunWorker(Queue<ProcessTask> batch, SubProcessing worker, CancellationTokenSource cts)
        {
            do
            {
                if (cts.IsCancellationRequested) break;
                // Dequeue a task and get the command
                if (batch.TryDequeue(out var processTask))
                {
                    var command = processTask.GetCommand();
                    // Check if the output lip folder exists, if not create it
                    if (!Directory.Exists(Path.GetDirectoryName(processTask.LipOutputPath)))
                    {
                        var target = Path.GetDirectoryName(processTask.LipOutputPath)!;
                        Directory.CreateDirectory(target);
                    }

                    // Start timer
                    // var timer = new System.Diagnostics.Stopwatch();
                    // timer.Start();
                    // Run the command
                    var sw1 = System.Diagnostics.Stopwatch.StartNew();
                    var result = worker.DoTask(command);
                    sw1.Stop();
                    System.Diagnostics.Debug.WriteLine(
                        $@"P [{worker.processNumber}], time: {sw1.ElapsedMilliseconds}ms");
                    if (!result) // If failed
                    {
                        // If failed, retry up to defined limit
                        if (processTask.RetryCount < MaxRetryCount)
                        {
                            batch.Enqueue(processTask);
                            processTask.RetryCount++;
                        }
                        else
                        {
                            Interlocked.Increment(ref errorCount);
                            Interlocked.Increment(ref ProcessedCount);
                        }
                    }
                    else
                    {
                        Interlocked.Increment(ref ProcessedCount);
                    }
                }
            } while (batch.Count > 0);
        }
    }
}
