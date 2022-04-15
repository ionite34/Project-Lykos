namespace Project_Lykos.Worker
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        //private readonly IConfiguration _Configuration;
        private readonly QueueHelper _queueHelper;
        private readonly IServiceProvider _services;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        //private SubProcess _externalProcess;
        public WindowsBackgroundService(
            IHostApplicationLifetime lifetime,
            IConfiguration configuration,
            QueueHelper queueService,
            IServiceProvider services,
            ILogger<WindowsBackgroundService> logger)
        {
            _lifetime = lifetime;   
            _queueHelper = queueService;
            _logger = logger;
            _services = services;

            configuration.Bind("QueueProcessor", _queueHelper);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting QueueProcessor {DateTimeOffset.Now}");
            bool first = true;
            string processName = "MainLoop";
            TimeSpan delay = TimeSpan.FromMinutes(1);
            SubProcessing processor = null;
            int processorIdleCounter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                using var dbContext = scope.ServiceProvider.GetService<Data.LykosQueueContext>();

                _logger.LogInformation($"{processName}: Next Batch {DateTimeOffset.Now}");
                var timer = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    processName = "DbContext";
                    if (first)
                    {
                        dbContext.Database.EnsureCreated();

                        processName = "ExternalProcess";
                        if (!DependencyCheck.CheckFonixData())
                        {
                            delay = _queueHelper.CriticalDelay;
                            _logger.LogCritical($"{processName} - Fonix data not found: {DependencyCheck.GetFonixDataPath()}");
                            _queueHelper.LogError(dbContext, processName, "Fonix data not found", DependencyCheck.GetFonixDataPath());
                            if (_queueHelper.BreakOnCritical)
                                throw new OperationCanceledException($"{processName} - Fonix data not found: {DependencyCheck.GetFonixDataPath()}");
                        }

                        try
                        {
                            Cache.Setup();
                            first = false;
                        }
                        catch (Exception ex)
                        {
                            delay = _queueHelper.CriticalDelay;
                            _logger.LogCritical($"{processName} - Failed to setup cache: {ex.Message}");
                            _queueHelper.LogError(dbContext, processName, "Fonix data not found", ex);
                            if (_queueHelper.BreakOnCritical)
                                throw new OperationCanceledException($"{processName} - Failed to setup cache: {ex.Message}");
                        }
                    }

                    processName = "Batch";
                    var batch = _queueHelper.GetNextBatch(dbContext);
                    Queue<ProcessTask> batchQueue = null;
                    if (batch?.Any() == true)
                    {
                        // reset idle counter
                        processorIdleCounter = 0;
                        // Only start the process if some of the items need it.
                        if (batch.Any(x => x.UseDllDirect.GetValueOrDefault() == false))
                        {
                            try
                            {
                                if (processor == null)
                                {
                                    processor = new SubProcessing(-1, false); // CS: don't write to file, we'll write to the database instead
                                }
                                if (!processor.IsRunning())
                                {
                                    var started = await Task.Run(() => processor.StartSubProcess());
                                    if (!started)
                                        throw new ApplicationException("StartSubProcess returned false");
                                }
                            }
                            catch (Exception ex)
                            {
                                delay = _queueHelper.CriticalDelay;
                                var logs = processor?.FetchAndClearOutputBuffer();
                                _logger.LogCritical("{processName}: Unable to start sub process : {logs}", processName, logs);
                                _queueHelper.LogError(dbContext, processName, "Unable to start sub process", ex, logs);
                                _queueHelper.ReturnAsCritical(dbContext, batch, "Unable to start sub process");
                                if (_queueHelper.BreakOnCritical)
                                    return;
                            }
                        }

                        batchQueue = new Queue<ProcessTask>(batch.Select(x => new ProcessTask(x)));
                        processName = "Processor";
                        while (batchQueue.TryDequeue(out var nextTask))
                        {
                            if (nextTask.DbEntry.UseDllDirect.GetValueOrDefault())
                            {
                                try
                                {
                                    int errorState = await nextTask.ProcessAsync(stoppingToken, TimeSpan.FromSeconds(15));
                                    // results will be written to the dbEntry
                                    if (errorState == 0)
                                        _queueHelper.MarkAsProcessed(dbContext, nextTask.DbEntry);
                                    else
                                        _queueHelper.MarkAsError(dbContext, nextTask.DbEntry, null);
                                }
                                catch (Exception ex)
                                {
                                    _queueHelper.MarkAsError(dbContext, nextTask.DbEntry, ex);
                                }
                            }
                            else
                            {
                                try
                                {
                                    var success = processor.DoTask(nextTask.GetCommand());
                                    nextTask.DbEntry.Runtime = processor.DoWorkTime;
                                    nextTask.DbEntry.Output = processor.FetchAndClearOutputBuffer();
                                    if(success)
                                        _queueHelper.MarkAsProcessed(dbContext, nextTask.DbEntry);
                                    else
                                        _queueHelper.MarkAsError(dbContext, nextTask.DbEntry, null);
                                }
                                catch (Exception ex)
                                {
                                    nextTask.DbEntry.Runtime = processor.DoWorkTime;
                                    nextTask.DbEntry.Output = processor.FetchAndClearOutputBuffer();
                                    _queueHelper.MarkAsError(dbContext, nextTask.DbEntry, ex);
                                }
                            }
                        }
                        if (stoppingToken.IsCancellationRequested)
                            break;
                    }

                    processName = "Finalizer";
                    // If process is cancelled or if it exits early for any other reason, then we should put the remaining items back onto the queue
                    var remaining = batchQueue?.Select(x => x.DbEntry).ToArray();
                    if (remaining?.Any() == true)
                    {
                        if (stoppingToken.IsCancellationRequested)
                        {
                            _logger.LogWarning($"{processName}: Batch Processing Aborted {DateTimeOffset.Now} ({timer.Elapsed})");
                            _queueHelper.ReturnAsCritical(dbContext, remaining, "Batch processing aborted");
                            // deliberately exit
                            return;
                        }
                        else
                        {
                            _logger.LogWarning($"{processName}: Batch Interrupted {DateTimeOffset.Now} ({timer.Elapsed})");
                            _queueHelper.ReturnAsCritical(dbContext, remaining, "Batch processing Interrupted");
                            // deliberately skip the rest of the logic
                            continue;
                        }
                    }
                    else if(batch?.Any() == true)
                    {
                        _logger.LogInformation($"{processName}: Batch Complete {DateTimeOffset.Now} ({timer.Elapsed})");

                        // deliberately skip the rest of the logic
                        continue;
                    }
                    else
                    {
                        if (processor != null)
                        {
                            if (++processorIdleCounter >= 5)
                            {
                                processor.StopAll();
                                // Pause for a second to let the subprocesses stop
                                await Task.Delay(1000);
                                processor = null;
                            }
                        }
                    }
                }
                catch (OperationCanceledException cancelX)
                {
                    // throw;
                    // these won't actually go anywhere!, so don't just throw!
                    // We can try to raise the cancel event from a new source, to force the cancellation, but that didn't work either
                    //CancellationTokenSource.CreateLinkedTokenSource(stoppingToken).Cancel();

                    _logger.LogInformation($"Ending QueueProcessor: {cancelX.Message}");
                    _lifetime.StopApplication();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed {processName}: {ex.Message}");
                    _queueHelper.LogError(dbContext, processName, $"Failed {processName}: {ex.Message}", ex);
                }

                processName = "MainLoop";
                _logger.LogInformation($"{processName}: Idle for another {delay}");
                await Task.Delay(delay, stoppingToken);
            }

            _logger.LogInformation($"Ending QueueProcessor {DateTimeOffset.Now}");
            _lifetime.StopApplication();
        }
    }
}