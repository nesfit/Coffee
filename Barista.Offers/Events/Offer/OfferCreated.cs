using System;
using Barista.Contracts.Events.Offer;

namespace Barista.Offers.Events.Offer
{
    public class OfferCreated : IOfferCreated
    {
        public OfferCreated(Guid id, Guid pointOfSaleId, Guid productId, decimal? recommendedPrice, Guid? stockItemId, DateTimeOffset? validSince, DateTimeOffset? validUntil)
        {
            Id = id;
            PointOfSaleId = pointOfSaleId;
            ProductId = productId;
            RecommendedPrice = recommendedPrice;
            StockItemId = stockItemId;
            ValidSince = validSince;
            ValidUntil = validUntil;
        }

        public Guid Id { get; }
        public Guid PointOfSaleId { get; }
        public Guid ProductId { get; }
        public decimal? RecommendedPrice { get; }
        public Guid? StockItemId { get; }
        public DateTimeOffset? ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
    }
}
