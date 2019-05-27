using System;
using Barista.Common;

namespace Barista.Api.Queries
{
    public class BrowseOffers : PagedQuery
    {
        public Guid? AtPointOfSaleId { get; set; }
        public Guid? OfProductId { get; set; }
        public Guid? OfStockItemId { get; set; }
        public DateTime? ValidAt { get; set; }
    }
}
