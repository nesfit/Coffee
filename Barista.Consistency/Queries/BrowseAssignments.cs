using System;
using Barista.Common;

namespace Barista.Consistency.Queries
{
    public class BrowseAssignments : PagedQuery
    {
        public Guid? AssignedToUser { get; set; }
        public Guid? AssignedToPointOfSale { get; set; }
        public Guid? OfAuthenticationMeans { get; set; }
    }
}