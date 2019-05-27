using System;
using System.Linq;
using Barista.Common;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Dto;

namespace Barista.PointsOfSale.Queries
{
    public class BrowsePointsOfSale : PagedQuery<PointOfSaleDto>, IPagedQueryImpl<PointOfSale>
    {
        public string DisplayName { get; set; }
        public Guid? ParentAccountingGroupId { get; set; }
        
        public IQueryable<PointOfSale> Apply(IQueryable<PointOfSale> q)
        {
            q = q.ApplySort(SortBy);

            if (DisplayName != null)
                q = q.Where(pos => pos.DisplayName.Contains(DisplayName));

            if (ParentAccountingGroupId is Guid val)
                q = q.Where(pos => pos.ParentAccountingGroupId == val);

            return q;
        }
    }
}
