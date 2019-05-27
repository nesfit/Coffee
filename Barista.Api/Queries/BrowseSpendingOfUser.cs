using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseSpendingOfUser : PagedQuery
    {
        public Guid UserId { get; set; }
        public DateTimeOffset? Since { get; set; }
        public DateTimeOffset? Until { get; set; }
        public Guid? AtPointOfSaleId { get; set; }
        public Guid? AtAccountingGroupId { get; set; }
        public bool IncludePayments { get; set; } = true;
    }
}
