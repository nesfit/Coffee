using System;
using System.Linq;
using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class BrowsePayments : PagedQuery<PaymentDto>, IPagedQueryImpl<Payment>
    {
        public Guid? CreditedToUser { get; set; }
        public DateTimeOffset? OlderThan { get; set; }
        public DateTimeOffset? NewerThan { get; set; }

        public IQueryable<Payment> Apply(IQueryable<Payment> q)
        {
            q = q.ApplySort(SortBy);

            if (CreditedToUser != null)
                q = q.Where(p => p.UserId == CreditedToUser);

            if (NewerThan != null)
                q = q.Where(p => p.Created > NewerThan);

            if (OlderThan != null)
                q = q.Where(p => p.Created < OlderThan);

            return q;
        }
    }
}
