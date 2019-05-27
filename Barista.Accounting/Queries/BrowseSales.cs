using System;
using System.Linq;
using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class BrowseSales : PagedQuery<SaleDto>, IPagedQueryImpl<Sale>
    {
        public Guid? MadeByPointOfSale { get; set; } 
        public Guid? MadeByUser { get; set; }
        public Guid? MadeInAccountingGroup { get; set; }
        public DateTimeOffset? OlderThan { get; set; }
        public DateTimeOffset? NewerThan { get; set; }
        
        public IQueryable<Sale> Apply(IQueryable<Sale> q)
        {
            q = q.ApplySort(SortBy);
            
            if (NewerThan != null)
                q = q.Where(s => s.Created > NewerThan);

            if (OlderThan != null)
                q = q.Where(s => s.Created < OlderThan);

            if (MadeByPointOfSale != null)
                q = q.Where(s => s.PointOfSaleId == MadeByPointOfSale);

            if (MadeByUser != null)
                q = q.Where(s => s.UserId == MadeByUser);

            if (MadeInAccountingGroup != null)
                q = q.Where(s => s.AccountingGroupId == MadeInAccountingGroup);

            return q;
        }
    }
}
