using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationEventLog
{
    public class IntegrationEventLogContext: DbContext
    {
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationEventLogEntry>().HasKey(e => e.EventId);
        }
    }
}
