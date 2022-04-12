using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CliWrap;

namespace Project_Lykos
{
    public class ProcessControl : Queue<ProcessTask>
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
        private int processedCount;
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
        
        // Constructor
        public ProcessControl()
        {
            
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
        public async Task Start(int processCount, int batchSize, bool useNativeResampling, CancellationTokenSource cts)
        {
            Cache.Setup();
            BatchSize = batchSize;
            TotalBatches = (int)Math.Ceiling((double)this.Count / batchSize);
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = processCount,
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
                    await ProcessBatch(batchQueue, processCount, cts, useNativeResampling);
                }
                catch (TaskCanceledException)
                {
                    throw new TaskCanceledException("Processing canceled.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while processing batch: " + ex.Message);
                }


                // Cleanup
                foreach (var processTask in currentBatch)
                {
                    // Delete Temp File
                    File.Delete(processTask.WavTempPath);
                }
            }
        }
        
        // New Process Batch
        private async Task ProcessBatch(Queue<ProcessTask> batch, int processCount, CancellationTokenSource cts, bool useNativeResampling)
        {
            errorCount = 0;
            timeOutCount = 0;
            processedCount = 0;
            lastReportedCount = 0;
            // Register to kill all subprocesses if the user cancels
            cts.Token.Register(() =>
            {
                foreach (var subProcess in Workers)
                {
                    subProcess.StopAll();
                }
                Cache.KillProcesses().Wait();
                throw new TaskCanceledException();
            });
            
            // Make new workers equal to the process count
            for (var i = 0; i < processCount; i++)
            {
                Workers.Add(new SubProcessing(this));
            }

            // Start subprocesses
            var tasks = new List<Task>();
            foreach (var worker in Workers)
            {
                var startTask = Task.Run(() => worker.StartSubProcess());
                tasks.Add(startTask);
            }
            await Task.WhenAll(tasks);
            var results = tasks.Select(task => ((Task<bool>) task).Result).ToList();
            // If any of the subprocesses failed, throw an exception
            if (results.Any(result => !result))
            {
                cts.Cancel();
                throw new Exception("Error while starting subprocesses.");
            }
            
            // Pause for a second to let the subprocesses start
            // await Task.Delay(1000);
            
            // Clear tasks
            tasks.Clear();
            // Add new tasks
            foreach (var worker in Workers)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        // Exit if queue empty
                        if (batch.Count == 0) return;
                        // Dequeue a task and get the command
                        var processTask = batch.Dequeue();
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
                        var result = worker.DoTask(command);
                        // If failed, retry up to 1 time
                        if (!result)
                        {
                            // If failed, retry up to 1 time
                            if (processTask.RetryCount < MaxRetryCount)
                            {   // For retry, put it back in the queue
                                batch.Enqueue(processTask);
                                processTask.RetryCount++;
                            }
                            else
                            {
                                // Increment process and error count
                                Interlocked.Increment(ref errorCount);
                                Interlocked.Increment(ref processedCount);
                            }
                        }
                        else
                        {
                            // Write time to cache
                            // timer.Stop();
                            // Cache.ProcessingTimes.Add(timer.ElapsedMilliseconds);
                            // Increment processed count
                            Interlocked.Increment(ref processedCount);
                        }
                        // Update Progress
                        CheckProgress();
                    }
                    if (cts.IsCancellationRequested)
                        throw new TaskCanceledException();
                }, TaskCreationOptions.LongRunning);
                tasks.Add(task);
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (TaskCanceledException ex)
            {
                throw new TaskCanceledException("Task was cancelled.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while running tasks.", ex);
            }


            while (batch.Count > 0)
            {
                //subProcess.StopAll();
            }
            
            Console.WriteLine(@"Processing complete.");
        }
        
        // Process single batch
        private void ProcessBatch_Legacy(IEnumerable<ProcessTask> batch, int processCount, CancellationTokenSource cts, bool useNativeResampling)
        {
            errorCount = 0;
            timeOutCount = 0;
            processedCount = 0;
            lastReportedCount = 0;

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = processCount,
                CancellationToken = cts.Token
            };
            
            Parallel.ForEach(batch, options, task =>
            {
                // Abort if process count is between 0 and 100 and error count exceeds 25
                var c1 = processedCount < 100 && errorCount > 25;
                // Also abort if process count more than 100 but error exceeds 30%
                var c2 = processedCount > 100 && errorCount > (processedCount / 4);
                if (c1 || c2)
                {
                    cts.Cancel();
                    throw new Exception("Too many errors, aborting");
                }
                // If using native resampling
                task.UseNativeResampler = useNativeResampling;
                var worker = task.ProcessAsync(cts.Token, TimeSpan.FromSeconds(15));
                var result = worker.Result;
                if (result is 1 or 2) Interlocked.Increment(ref this.errorCount); // Error includes timeout
                if (result is 2) Interlocked.Increment(ref this.timeOutCount); // Timeout count
                Interlocked.Increment(ref this.processedCount); // Total processed count
                CheckProgress(); // Check progress
            });
        }
    }   
}
