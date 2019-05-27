using System;

namespace Barista.Api.Models.Offers
{
    public class Offer
    {
        public Guid Id { get; set; }
        public Guid PointOfSaleId { get; set; }
        public Guid ProductId { get; set; }
        public decimal? RecommendedPrice { get; set; }
        public Guid? StockItemId { get; set; }
        public DateTimeOffset? ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}
