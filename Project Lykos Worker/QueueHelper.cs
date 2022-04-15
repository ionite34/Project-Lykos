using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project_Lykos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Worker
{
    public class QueueHelper
    {
        public string MachineName { get; set; }
        public int ProcessId { get; set; }
        public int BatchSize { get; set; }
        public bool LogGetNextBatch { get; set; }
        /// <summary>
        /// Timespan to wait before getting the next batch when there were no items in the queue to process
        /// </summary>
        public TimeSpan IdleDelay { get; set; }
        /// <summary>
        /// Timespan to wait before retry after a critical error
        /// </summary>
        public TimeSpan CriticalDelay { get; set; }
        /// <summary>
        /// Flag to indicate the service will stop if a critical error is observed
        /// </summary>
        public bool BreakOnCritical { get; set; }


        public QueueHelper()
        {
            MachineName = Environment.MachineName;
            ProcessId = Environment.ProcessId;
            LogGetNextBatch = true;
            BreakOnCritical = true;
            IdleDelay = TimeSpan.FromMinutes(1);
            CriticalDelay = TimeSpan.FromMinutes(10);
        }



        /// <summary>
        /// Get the next <paramref name="records"/> number of records from the queue as a batch
        /// </summary>
        /// <param name="dbContext">Database context to query</param>
        /// <param name="records">Number of records to fetch from the queue</param>
        /// <returns>list of records from the queue</returns>
        public IEnumerable<Project_Lykos.Data.QueueEntry> GetNextBatch(Data.LykosQueueContext dbContext, int? records = null)
        {
            if (records == null || records <= 0)
                records = BatchSize;
            if (records <= 0)
                records = 1;


            IEnumerable<Project_Lykos.Data.QueueEntry> batch = dbContext.QueueEntries.FromSqlRaw(@"
  DECLARE @maxDequeued DateTimeOffset = (SELECT DATEADD(MI, @maxMinutes, SYSDATETIMEOFFSET()));
  DECLARE @outIds Table (
	Id INT NOT NULL
  )
  UPDATE QueueEntries SET
	ErrorCount = CASE WHEN Dequeued IS NULL THEN ErrorCount
                      WHEN ErrorCount IS NULL THEN 1
                      ELSE ErrorCount + 1
                 END,
	ErrorMessage = CASE WHEN Dequeued IS NULL THEN NULL
	                    ELSE 'Timeout - Retry'
                   END,
    Dequeued = SYSDATETIMEOFFSET(),
	LastDequeued = SYSDATETIMEOFFSET()
  OUTPUT inserted.Id
  INTO @outIds
  FROM QueueEntries
  WHERE Id IN
  (
	SELECT TOP (@rows) Id
	FROM QueueEntries WITH (updlock, readpast)
	WHERE Processed IS NULL 
	  AND (ErrorCount IS NULL OR ErrorCount <= @maxErrors) 
	  AND (Dequeued IS NULL OR Dequeued < @maxDequeued)
	ORDER BY Id
  );
  SELECT * FROM QueueEntries
  WHERE Id IN (SELECT Id FROM @outIds)",
  new SqlParameter("rows", records),
  new SqlParameter("maxMinutes", -2),
  new SqlParameter("maxErrors", 3)
  ).ToArray();

            if (batch?.Any() == true && LogGetNextBatch)
            {
                LogInfo(dbContext, "GetNextBatch", $"Dequeued {batch.Count()} records");
            }
            return batch;
        }

        /// <summary>
        /// Mark an entry as failed, effectively putting the item back onto the queue for re-processing
        /// </summary>
        /// <param name="record"></param>
        /// <param name="error"></param>
        public void MarkAsError(Data.LykosQueueContext dbContext, Project_Lykos.Data.QueueEntry record, Exception error)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        public void MarkAsProcessed(Data.LykosQueueContext dbContext, Project_Lykos.Data.QueueEntry record)
        {

        }

        /// <summary>
        /// Due to global critical error, return items back to the queue, reset their error count to 1
        /// </summary>
        /// <param name="dbContext">Database context to operate against</param>
        /// <param name="batch">Entries that we need ot put back on the queue</param>
        /// <param name="errorMessage">Message to log against each item</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReturnAsCritical(LykosQueueContext dbContext, IEnumerable<QueueEntry> batch, string errorMessage)
        {
            foreach(var entry in batch)
            {
                entry.ErrorCount = 1;
                entry.ErrorMessage = errorMessage;
                entry.Dequeued = null;
                entry.Processed = null;
                dbContext.Entry(entry).State = EntityState.Modified;
            }
            dbContext.SaveChanges();
        }

        public void LogInfo(Data.LykosQueueContext dbContext, string process, string message, string detail = null, int? queueEntryId = null)
        {
            dbContext.LogEntries.Add(new Data.LogEntry
            {
                EntryDate = DateTimeOffset.Now,
                MachineName = this.MachineName,
                Process = process,
                QueueEntryId = queueEntryId,
                Message = message.Truncate(100),
                Detail = detail,
                Severity = Data.SeverityLevel.Information
            });
            dbContext.SaveChanges();
        }
        public void LogError(Data.LykosQueueContext dbContext, string process, string message, Exception ex, string details = null, int? queueEntryId = null)
        {
            details = details ?? "";
            if (ex != null) details = ex.ToString() + (String.IsNullOrWhiteSpace(details) ? String.Empty : Environment.NewLine + details); 
            dbContext.LogEntries.Add(new Data.LogEntry
            {
                EntryDate = DateTimeOffset.Now,
                MachineName = this.MachineName,
                Process = process,
                QueueEntryId = queueEntryId,
                Message = message.Truncate(100),
                Detail = ex.ToString(),
                Severity = Data.SeverityLevel.Error
            });
            dbContext.SaveChanges();
        }
        public void LogError(Data.LykosQueueContext dbContext, string process, string message, string details, int? queueEntryId = null)
        {
            dbContext.LogEntries.Add(new Data.LogEntry
            {
                EntryDate = DateTimeOffset.Now,
                MachineName = this.MachineName,
                Process = process,
                QueueEntryId = queueEntryId,
                Message = message.Truncate(100),
                Detail = details,
                Severity = Data.SeverityLevel.Error
            });
            dbContext.SaveChanges();
        }

    }

}
