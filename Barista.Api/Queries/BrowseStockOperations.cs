using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseStockOperations : PagedQuery
    {
        public Guid[] StockItemId { get; set; }
    }
}
