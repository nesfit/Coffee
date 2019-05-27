using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseSales : PagedQuery
    {
        public Guid? MadeByPointOfSale { get; set; } 
        public Guid? MadeByUser { get; set; }
        public Guid? MadeInAccountingGroup { get; set; }
        public DateTimeOffset? OlderThan { get; set; }
        public DateTimeOffset? NewerThan { get; set; }
    }
}
