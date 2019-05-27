using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowsePayments : PagedQuery
    {
        public Guid? CreditedToUser { get; set; }
        public DateTimeOffset? OlderThan { get; set; }
        public DateTimeOffset? NewerThan { get; set; }
    }
}
