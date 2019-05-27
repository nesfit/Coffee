using System;
using System.Linq;
using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class BrowseSaleStateChanges : PagedQuery<SaleStateChangeDto>, IPagedQueryImpl<SaleStateChange>
    {
        public BrowseSaleStateChanges(Guid parentSaleId)
        {
            ParentSaleId = parentSaleId;
        }

        public Guid ParentSaleId { get; }
        public IQueryable<SaleStateChange> Apply(IQueryable<SaleStateChange> q) => q.ApplySort(SortBy);
    }
}
