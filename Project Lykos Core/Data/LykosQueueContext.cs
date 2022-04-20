using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos.Data
{
    public class LykosQueueContext : DbContext
    {
        public LykosQueueContext(DbContextOptions<LykosQueueContext> options)
            : base(options)
        {
        }

        public LykosQueueContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("name=LykosQueue");
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LykosQueue"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                optionsBuilder.UseSqlServer("name=LykosQueue");
            else
                optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Data.QueueEntry> QueueEntries { get; set; }
        public DbSet<Data.LogEntry> LogEntries { get; set; }
    }
}
