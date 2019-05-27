using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseDetailedUsers : PagedQuery
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsActive { get; set; }
    }
}
