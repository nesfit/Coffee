using System;
using Barista.Contracts.Commands.Offer;

namespace Barista.Api.Commands.Offer
{
    public class CreateOffer : ICreateOffer
    {
        public CreateOffer(Guid id, Guid pointOfSaleId, Guid productId, decimal? recommendedPrice, Guid? stockItemId, DateTimeOffset? validSince, DateTimeOffset? validUntil)
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
