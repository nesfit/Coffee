using System;
using System.Linq;
using Barista.Common;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseSpendingLimits : PagedQuery<SpendingLimitDto>, IPagedQueryImpl<SpendingLimit>
    {
        public Guid ParentAssignmentToUserId { get; set; }

        public IQueryable<SpendingLimit> Apply(IQueryable<SpendingLimit> q) => q.OrderBy(sl => sl.Id);
    }
}
