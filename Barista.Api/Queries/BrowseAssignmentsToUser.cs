using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseAssignmentsToUser : PagedQuery
    {
        public Guid? OfAuthenticationMeans { get; set; }
        public bool MustBeValid { get; set; } = false;
        public Guid? AssignedToUser { get; set; }
    }
}
