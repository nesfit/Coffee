using System;
using System.Linq;
using Barista.Common;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAssignmentsToUser : PagedQuery<AssignmentToUserDto>, IPagedQueryImpl<AssignmentToUser>
    {
        public Guid? OfAuthenticationMeans { get; set; }
        public bool MustBeValid { get; set; } = false;
        public Guid? AssignedToUser { get; set; }
        
        public IQueryable<AssignmentToUser> Apply(IQueryable<AssignmentToUser> q)
        {
            q = q.ApplySort(SortBy);

            if (OfAuthenticationMeans is Guid meansId)
                q = q.Where(a => a.MeansId == meansId);

            if (MustBeValid)
            {
                var now = DateTimeOffset.UtcNow;
                q = q.Where(a => a.ValidSince == null || now > a.ValidSince);
                q = q.Where(a => a.ValidUntil == null || now < a.ValidUntil);
            }

            if (AssignedToUser is Guid userId)
                q = q.Where(a => a.UserId == userId);

            return q;
        }
    }
}
