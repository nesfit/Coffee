using Barista.Common.EfCore;
using Barista.StockItems.Domain;

namespace Barista.StockItems.Repositories
{
    public class StockItemRepository : CrudRepository<StockItemsDbContext, StockItem>, IStockItemRepository
    {
        public StockItemRepository(StockItemsDbContext dbContext) : base(dbContext, dbc => dbc.StockItems)
        {
        }
    }
}
