using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowsePointOfSaleAuthorizedUsers : PagedQuery
    {
        public Guid? UserId { get; set; }
        public Guid? PointOfSaleId { get; set; }
        public string UserAuthorizationLevel { get; set; }
    }
}
