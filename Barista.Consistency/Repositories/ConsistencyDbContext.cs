using System;
using Barista.Consistency.Domain;
using Barista.Consistency.Events;
using Barista.Contracts.Events.Consistency;
using Barista.Contracts.Events.Sale;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Barista.Consistency.Repositories
{
    public class ConsistencyDbContext : DbContext
    {
        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }

        public ConsistencyDbContext()
        {

        }

        public ConsistencyDbContext(DbContextOptions<ConsistencyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ScheduledEvent>().HasIndex(msg => msg.ScheduledFor);
            modelBuilder.Entity<ScheduledEvent>().HasData(InitializationEventFactory());
        }

        protected static ScheduledEvent InitializationEventFactory()
        {
            return new ScheduledEvent(
                Guid.NewGuid(),
                typeof(IDatabaseCreated).AssemblyQualifiedName,
                "{}",
                DateTimeOffset.UtcNow
            );
        }
    }
}
