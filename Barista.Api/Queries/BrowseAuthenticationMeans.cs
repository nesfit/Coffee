using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseAuthenticationMeans : PagedQuery
    {
        public string Method { get; set; }
        public string Value { get; set; }
    }
}
