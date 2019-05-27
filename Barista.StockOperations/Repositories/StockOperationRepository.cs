using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;
using Barista.StockOperations.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.StockOperations.Repositories
{
    public class StockOperationRepository : CrudRepository<StockOperationsDbContext, StockOperation>, IStockOperationRepository
    {
        public StockOperationRepository(StockOperationsDbContext dbContext) : base(dbContext, dbc => dbc.StockOperations)
        {
        }

        public async Task<IPagedResult<ManualStockOperation>> BrowseAsync(IPagedQueryImpl<ManualStockOperation> pagedQuery)
        {
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));
            return await DbContext.StockOperations.OfType<ManualStockOperation>().PaginateAsync(pagedQuery);
        }

        public async Task<IPagedResult<SaleBasedStockOperation>> BrowseAsync(IPagedQueryImpl<SaleBasedStockOperation> pagedQuery)
        {
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));
            return await DbContext.StockOperations.OfType<SaleBasedStockOperation>().PaginateAsync(pagedQuery);
        }

        public async Task<decimal> GetStockItemBalance(Guid id)
        {
            return await DbContext.StockOperations.Where(op => op.StockItemId == id).SumAsync(op => op.Quantity);
        }
    }
}
