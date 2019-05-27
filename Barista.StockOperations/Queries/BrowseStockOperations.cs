using System;
using System.Linq;
using Barista.Common;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class BrowseStockOperations : PagedQuery<StockOperationDto>, IPagedQueryImpl<StockOperation>
    {
        public Guid[] StockItemId { get; set; } = new Guid[0];

        public IQueryable<StockOperation> Apply(IQueryable<StockOperation> q)
        {
            q = q.ApplySort(SortBy);

            if (StockItemId.Any())
            {
                var stockItemIds = StockItemId;
                q = q.Where(stockOp => stockItemIds.Contains(stockOp.StockItemId));
            }

            return q;
        }
    }
}
