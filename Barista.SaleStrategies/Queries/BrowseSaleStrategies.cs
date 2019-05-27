using System.Linq;
using Barista.Common;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Dto;

namespace Barista.SaleStrategies.Queries
{
    public class BrowseSaleStrategies : PagedQuery<SaleStrategyDto>, IPagedQueryImpl<SaleStrategy>
    {
        public string DisplayName { get; set; }

        public IQueryable<SaleStrategy> Apply(IQueryable<SaleStrategy> q)
        {
            q = q.ApplySort(SortBy);

            if (DisplayName != null)
                q = q.Where(ss => ss.DisplayName.Contains(DisplayName));

            return q;
        }
    }
}
