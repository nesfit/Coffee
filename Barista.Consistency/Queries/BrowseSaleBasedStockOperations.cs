using System;
using Barista.Common;

namespace Barista.Consistency.Queries
{
    public class BrowseSaleBasedStockOperations : PagedQuery
    {
        public Guid[] StockItemId { get; set; }
        public Guid? SaleId { get; set; }
    }
}
