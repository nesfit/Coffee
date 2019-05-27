using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.StockOperations.Domain;

namespace Barista.StockOperations.Repositories
{
    public interface IStockOperationRepository : IBrowsableRepository<StockOperation>, IBrowsableRepository<ManualStockOperation>, IBrowsableRepository<SaleBasedStockOperation>
    {
        Task<StockOperation> GetAsync(Guid id);
        Task<decimal> GetStockItemBalance(Guid id);
        Task AddAsync(StockOperation stockOperation);
        Task UpdateAsync(StockOperation stockOperation);
        Task DeleteAsync(StockOperation stockOperation);
        Task SaveChanges();
    }
}
