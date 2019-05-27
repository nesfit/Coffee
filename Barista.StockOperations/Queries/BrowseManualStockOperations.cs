using System;
using System.Linq;
using Barista.Common;
using Barista.StockOperations.Domain;
using Barista.StockOperations.Dto;

namespace Barista.StockOperations.Queries
{
    public class BrowseManualStockOperations : PagedQuery<ManualStockOperationDto>, IPagedQueryImpl<ManualStockOperation>
    {
        public Guid[] StockItemId { get; set; } = new Guid[0];
        public Guid? ByUserId { get; set; }

        public IQueryable<ManualStockOperation> Apply(IQueryable<ManualStockOperation> q)
        {
            q = q.ApplySort(SortBy);

            if (StockItemId.Any())
            {
                var stockItemIds = StockItemId;
                q = q.Where(stockOp => stockItemIds.Contains(stockOp.StockItemId));
            }

            if (ByUserId is Guid userId)
                q = q.Where(stockOp => stockOp.CreatedByUserId == userId);

            return q;
        }
    }
}
