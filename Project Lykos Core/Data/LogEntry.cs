using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Data
{
    /// <summary>
    /// Log an event in a central database
    /// </summary>
    /// <remarks>This is a simple implementation to enable logging of services deployed to different workstations</remarks>
    public class LogEntry
    {
        /// <summary>
        /// Primary Key for the Log Entry
        /// </summary>
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Timestamp that this record was created
        /// </summary>
        public DateTimeOffset EntryDate { get; set; }
        /// <summary>
        /// Name of the machine that the worker was executing on
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// Name of the process that logged the message
        /// </summary>
        public string Process { get; set; }
        /// <summary>
        /// Severity of the Log Message
        /// </summary>
        public SeverityLevel? Severity { get; set; }
        /// <summary>
        /// Short message for the log
        /// </summary>
        [StringLength(100)]
        public string? Message { get; set; }
        /// <summary>
        /// Extended details to go with the message
        /// </summary>
        public string? Detail { get; set; }
        /// <summary>
        /// Id of the QueueEntry record that is associated with this log... if there is one ;)
        /// </summary>
        public int? QueueEntryId { get; set; }

    }

    public enum SeverityLevel
    {
        Unknown = 0,
        /// <summary>
        /// Critical Error, Events that demand the immediate attention of the system administrator. They are generally directed at the global (system-wide) level, such as System or Application. They can also be used to indicate that an application or system has failed or stopped responding.
        /// </summary>
        Critical = 30,
        /// <summary>
        /// Events that indicate problems, but in a category that does not require immediate attention.
        /// </summary>
        Error = 40,
        /// <summary>
        /// Events that provide forewarning of potential problems; although not a response to an actual error, a warning indicates that a component or application is not in an ideal state and that some further actions could result in a critical error.
        /// </summary>
        Warning = 50,
        /// <summary>
        /// Events that pass noncritical information to the administrator, similar to a note that says: "For your information."
        /// </summary>
        Information = 80,
        /// <summary>
        /// Verbose status, such as progress or success messages.
        /// </summary>
        Verbose = 100

    }
}
