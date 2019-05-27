using System;
using System.Linq;
using Barista.Common;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class BrowseSaleBasedStockOperations : PagedQuery<SaleBasedStockOperationDto>, IPagedQueryImpl<SaleBasedStockOperation>
    {
        public Guid[] StockItemId { get; set; } = new Guid[0];
        public Guid? SaleId { get; set; }

        public IQueryable<SaleBasedStockOperation> Apply(IQueryable<SaleBasedStockOperation> q)
        {
            q = q.ApplySort(SortBy);

            if (StockItemId.Any())
            {
                var stockItemIds = StockItemId;
                q = q.Where(stockOp => stockItemIds.Contains(stockOp.StockItemId));
            }

            if (SaleId is Guid saleId)
                q = q.Where(stockOp => stockOp.SaleId == saleId);

            return q;
        }
    }
}
