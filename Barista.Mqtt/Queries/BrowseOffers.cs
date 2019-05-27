using System;
using Barista.Common;

namespace Barista.Mqtt.Queries
{
    public class BrowseOffers : PagedQuery
    {
        public Guid AtPointOfSaleId { get; set; }
        public DateTime ValidAt { get; set; }
    }
}
