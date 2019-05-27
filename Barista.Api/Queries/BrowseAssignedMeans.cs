using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseAssignedMeans : PagedQuery
    {
        public string Method { get; set; }
        public bool IsValid { get; set; }
    }
}
