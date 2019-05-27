using System.Collections.Generic;
using Barista.Operations.Tracking;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Barista.Operations.Repository
{
    public class OperationsDbContext : DbContext
    {
        public DbSet<RoutingSlipState> RoutingSlipStates { get; set; }

        public OperationsDbContext() : base()
        {

        }

        public OperationsDbContext(DbContextOptions options) : base(options)
        {

        }
        
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoutingSlipState>().HasKey(s => s.CorrelationId);
            modelBuilder.Entity<RoutingSlipState>().Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
