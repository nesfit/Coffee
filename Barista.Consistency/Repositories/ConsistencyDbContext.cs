using Barista.Consistency.Domain;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
