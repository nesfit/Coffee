using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseAssignmentsToPointOfSale : PagedQuery
    {
        public Guid? OfAuthenticationMeans { get; set; }
        public bool MustBeValid { get; set; } = false;
        public Guid? AssignedToPointOfSale { get; set; }
    }
}
