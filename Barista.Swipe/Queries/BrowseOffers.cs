using System;
using Barista.Common;

namespace Barista.Swipe.Queries
{
    public class BrowseOffers : PagedQuery
    {
        public Guid? AtPointOfSaleId { get; set; }
        public Guid? OfProductId { get; set; }
        public DateTime? ValidAt { get; set; }
    }
}
