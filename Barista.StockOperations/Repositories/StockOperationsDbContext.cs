using Barista.StockOperations.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.StockOperations.Repositories
{
    public class StockOperationsDbContext : DbContext
    {
        public StockOperationsDbContext() : base()
        {
        }

        public StockOperationsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<StockOperation> StockOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ManualStockOperation>();
            modelBuilder.Entity<SaleBasedStockOperation>();
        }
    }
}
