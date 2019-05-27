using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseUsers : PagedQuery
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddressExact { get; set; }
    }
}
