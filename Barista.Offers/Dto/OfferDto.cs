using System;

namespace Barista.Offers.Dto
{
    public class OfferDto
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
