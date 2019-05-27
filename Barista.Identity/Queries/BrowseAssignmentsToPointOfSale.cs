using System;
using System.Linq;
using Barista.Common;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAssignmentsToPointOfSale : PagedQuery<AssignmentToPointOfSaleDto>, IPagedQueryImpl<AssignmentToPointOfSale>
    {
        public Guid? OfAuthenticationMeans { get; set; }
        public bool MustBeValid { get; set; } = false;
        public Guid? AssignedToPointOfSale { get; set; }
        
        public IQueryable<AssignmentToPointOfSale> Apply(IQueryable<AssignmentToPointOfSale> q)
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

            if (AssignedToPointOfSale is Guid posId)
                q = q.Where(a => a.PointOfSaleId == posId);

            return q;
        }
    }
}
