using System;

namespace Barista.Consistency.Models
{
    public class Offer
    {
        public Guid Id { get; set; }
        public Guid PointOfSaleId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? StockItemId { get; set; }
    }
}
