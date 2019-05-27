using System;
using Barista.Common;

namespace Barista.Swipe.Queries
{
    public class BrowseAssignments : PagedQuery
    {
        public Guid? AssignedToUser { get; set; }
        public Guid? OfAuthenticationMeans { get; set; }
    }
}
