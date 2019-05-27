using System;
using System.Linq;
using Barista.Common;
using Barista.StockItems.Domain;
using Barista.StockItems.Dto;

namespace Barista.StockItems.Queries
{
    public class BrowseStockItems : PagedQuery<StockItemDto>, IPagedQueryImpl<StockItem>
    {
        public string DisplayName { get; set; }
        public Guid? AtPointOfSaleId { get; set; }

        public IQueryable<StockItem> Apply(IQueryable<StockItem> q)
        {
            q = q.ApplySort(SortBy);

            if (DisplayName != null)
                q = q.Where(si => si.DisplayName.Contains(DisplayName));

            if (AtPointOfSaleId is Guid posId)
                q = q.Where(si => si.PointOfSaleId == posId);

            return q;
        }
    }
}
