using Barista.StockItems.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.StockItems.Repositories
{
    public class StockItemsDbContext : DbContext
    {
        public StockItemsDbContext() : base()
        {  
        }

        public StockItemsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<StockItem> StockItems { get; set; }
    }
}
