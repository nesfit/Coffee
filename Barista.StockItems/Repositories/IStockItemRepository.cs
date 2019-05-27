using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.StockItems.Domain;

namespace Barista.StockItems.Repositories
{
    public interface IStockItemRepository : IBrowsableRepository<StockItem>
    {
        Task<StockItem> GetAsync(Guid id);
        Task AddAsync(StockItem stockItem);
        Task UpdateAsync(StockItem stockItem);
        Task DeleteAsync(StockItem stockItem);
        Task SaveChanges();
    }
}
