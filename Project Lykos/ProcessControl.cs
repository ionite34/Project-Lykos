namespace Project_Lykos
{
    public class ProcessControl : Queue<ProcessTask>
    {
        // Events
        public event Action<int, int>? ProgressChanged;
        public event Action<string>? Report;

        // Settings
        public bool UseNativeResampler { get; set; } = false;
        public int MaxRetryCount { get; set; } = 1; // Max number of retries for a failed task, 0 = no retry

        // Batch Indicators
        public int BatchSize { get; private set; }
        public int CurrentBatch { get; private set; }
        public int TotalBatches { get; private set; }
        public int TotalFiles { get; private set; }

        // Inner Batch Indicators
        private int errorCount;
        public int ProcessedCount;
        public int TotalProcessed;
        private int lastReportedCount;

        // List of workers
        private List<SubProcessingAdv> Workers { get; } = new List<SubProcessingAdv>();

        private void OnProgressChanged(int progressCount) // Progress Changed Event
        {
            ProgressChanged?.Invoke(progressCount, CurrentBatch);
        }
        
        private void SendReport(string report) // Report Event
        {
            Report?.Invoke(report);
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
            TotalFiles = Count;
            while (this.Count > 0)
            {
                if (cts.IsCancellationRequested) throw new OperationCanceledException();
                CurrentBatch++;

                // Reset counters
                errorCount = 0;
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
                        var options = new ParallelOptions()
                        {
                            MaxDegreeOfParallelism = processCount,
                            CancellationToken = cts.Token
                        };
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
                        await Task.Run(() => StartWorkers(cts, processCount));
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
        private void StartWorkers(CancellationTokenSource cts, int processCount)
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
                var affinity = i;
                // Disable affinity if only 1 Worker
                if (Workers.Count == 1) affinity = 0;
                Workers.Add(new SubProcessingAdv(affinity, UseNativeResampler));
            }

            // Start subprocesses
            var tasks = new List<Task<int>>();
            foreach (var subProcess in Workers)
            {
                tasks.Add(Task.Run(() => subProcess.StartSubProcess()));
            }
            // Run 
            Task.WhenAll(tasks);
            // Check results
            var results = tasks.Select(task => task.Result).ToList();
            
            // If any of the subprocesses failed, throw an exception
            if (results.Any(result => result != 1))
            {
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
            {
                var factoryTask = Task.Factory.StartNew(() =>
                {
                    RunWorker(batch, worker, cts);
                }, TaskCreationOptions.LongRunning);
                tasks.Add(factoryTask);
            }
            await Task.WhenAll(tasks);
        }

        private void RunWorker(Queue<ProcessTask> batch, SubProcessingAdv worker, CancellationTokenSource cts)
        {
            do
            {
                // Break if the user cancels
                if (cts.IsCancellationRequested) break;
                
                // Dequeue a task and get the command
                if (!batch.TryDequeue(out var processTask)) continue;
                var command = processTask.GetCommand();
                
                // Check if the output lip folder exists, if not create it
                if (!Directory.Exists(Path.GetDirectoryName(processTask.LipOutputPath)))
                {
                    var target = Path.GetDirectoryName(processTask.LipOutputPath)!;
                    Directory.CreateDirectory(target);
                }
                
                // Run the command
                var result = worker.DoTask(command);

                // Check result
                if (result != 1) // If failed
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
                        Interlocked.Increment(ref TotalProcessed);
                    }
                }
                else // If successful
                {
                    Interlocked.Increment(ref ProcessedCount);
                    Interlocked.Increment(ref TotalProcessed);
                }
            } while (batch.Count > 0);
        }
    }
}
