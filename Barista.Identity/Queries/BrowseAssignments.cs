using System;
using System.Linq;
using System.Linq.Expressions;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAssignments : PagedQuery<AssignmentDto>, IPagedQueryImpl<Assignment>
    {
        public Guid? OfAuthenticationMeans { get; set; }
        public bool MustBeValid { get; set; } = false;

        public IQueryable<Assignment> Apply(IQueryable<Assignment> q)
        {
            q = q.ApplySort(SortBy);

            if (OfAuthenticationMeans != null)
                q = q.Where(a => a.MeansId == OfAuthenticationMeans);

            if (MustBeValid)
            {
                var now = DateTimeOffset.UtcNow;
                q = q.Where(a => a.ValidSince == null || now > a.ValidSince);
                q = q.Where(a => a.ValidUntil == null || now < a.ValidUntil);
            }

            return q;
        }
    }
}
