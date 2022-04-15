using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Data
{
    /// <summary>
    /// Represents an entry in the queue maintained in a central database
    /// </summary>
    public class QueueEntry
    {
        /// <summary>
        /// Primary Key for the Queue Entry
        /// </summary>
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Timestamp that this record was created
        /// </summary>
        public DateTimeOffset Enqueued { get; set; } = DateTimeOffset.Now;
        /// <summary>
        /// Timestamp that this record was selected for processing
        /// </summary>
        /// <remarks>This does not indicate success, only that the item was attempted, if the records is not finished processing within X minutes then assume the process has failed</remarks>
        public DateTimeOffset? Dequeued { get; set; }
        /// <summary>
        /// Timestamp that this record was last selected for processing
        /// </summary>
        /// <remarks>This is for error diagnostics, <see cref="Dequeued"/> will be reset back to null if the item it returned to the queue</remarks>
        public DateTimeOffset? LastDequeued { get; set; }
        /// <summary>
        /// Timestamp that this record has completed processing, this record is no longer on the queue ;)
        /// </summary>
        /// <remarks>These records can be cleaned up, we don't need them any more</remarks>
        public DateTimeOffset? Processed { get; set; }
        /// <summary>
        /// Number of times that this record has failed in processing
        /// </summary>
        /// <remarks>After 5 times we really should abort the attemp and not retry</remarks>
        public int? ErrorCount { get; set; }
        /// <summary>
        /// If the Processing has Failed, the exception message should be shown here
        /// </summary>
        public string? ErrorMessage { get; set; }


        // Essential Parameters
        public string? WavSourcePath { get; set; }
        public string? LipOutputPath { get; set; }
        public string? Text { get; set; }
        public bool? UseDllDirect { get; set; }
        public bool? UseNativeResampler { get; set; }
        public string? WavTempPath { get; set; }
        
        [StringLength(40)]
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string? Command { get; set; }

        public TimeSpan? Runtime { get; set; }
        public int? ExitCode { get; set; }
        public string? Output { get; set; }

        public string FormatLogText()
        {
            var log = new StringBuilder();
            log.AppendLine("==========================================================");
            log.AppendLine($"Enqueued: {Enqueued}");
            log.AppendLine($"Id: [{Id}] {UUID}");
            log.AppendLine($"Dequeued: {LastDequeued}");
            log.AppendLine($"Processed: {Processed}");
            log.AppendLine("==========================================================");
            log.AppendLine($"Processing mode: {(UseDllDirect.GetValueOrDefault() ? "DLL Direct" : "External Process")}");
            log.AppendLine($"Resampling mode: {(UseNativeResampler.GetValueOrDefault() ? "Native" : "Custom")}");
            log.AppendLine($"Path to Audio: {WavSourcePath}");
            log.AppendLine($"Path to Resampled Audio: {WavTempPath}");
            log.AppendLine($"Path to Output: {LipOutputPath}");
            log.AppendLine($"Audio File Name: {(String.IsNullOrWhiteSpace(WavSourcePath) ? String.Empty : Path.GetFileName(WavSourcePath))}");
            log.AppendLine($"Text: {Text}");
            log.AppendLine("==========================================================");
            log.AppendLine($"Exit Code: {ExitCode}");
            log.AppendLine($"Run Time: {Runtime}");
            log.AppendLine("==========================================================");
            log.AppendLine("FaceFX output:");
            log.AppendLine(Output ?? String.Empty);
            log.AppendLine("==========================================================");
            log.AppendLine($"Error Count: {Processed}");
            log.AppendLine(ErrorMessage ?? string.Empty);
            log.AppendLine("==========================================================");
            return log.ToString();
        }

    }
}
