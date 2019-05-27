using System;
using Barista.Common;

namespace Barista.Consistency.Queries
{
    public class BrowseManualStockOperations : PagedQuery
    {
        public Guid[] StockItemId { get; set; }
    }
}
