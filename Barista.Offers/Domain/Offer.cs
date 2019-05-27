using System;
using Barista.Common;

namespace Barista.Offers.Domain
{
    public class Offer : Entity
    {
        public Guid PointOfSaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal? RecommendedPrice { get; private set; }
        public Guid? StockItemId { get; private set; }
        public DateTimeOffset? ValidSince { get; private set; }
        public DateTimeOffset? ValidUntil { get; private set; }

        public Offer(Guid id, Guid pointOfSaleId, Guid productId, decimal? recommendedPrice, Guid? stockItemId, DateTimeOffset? validSince, DateTimeOffset? validUntil) : base(id)
        {
            SetPointOfSaleId(pointOfSaleId);
            SetProductId(productId);
            SetRecommendedPrice(recommendedPrice);
            SetStockItemId(stockItemId);
            SetValidity(validSince, validUntil);
        }

        public void SetPointOfSaleId(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
            SetUpdatedNow();
        }

        public void SetProductId(Guid productId)
        {
            ProductId = productId;
            SetUpdatedNow();
        }

        public void SetRecommendedPrice(decimal? recommendedPrice)
        {
            if (recommendedPrice < decimal.Zero)
                throw new BaristaException("invalid_recommended_price", "Recommended price cannot be a negative number");

            RecommendedPrice = recommendedPrice;
            SetUpdatedNow();
        }

        public void SetStockItemId(Guid? stockItemId)
        {
            StockItemId = stockItemId;
            SetUpdatedNow();
        }

        public void SetValidity(DateTimeOffset? validSince, DateTimeOffset? validUntil)
        {
            if (validSince != null && validUntil != null && validSince > validUntil)
                throw new BaristaException("invalid_validity", $"The validity period cannot start after it has ended, {nameof(ValidSince)} must be older than {nameof(ValidUntil)}");

            ValidSince = validSince;
            ValidUntil = validUntil;
            SetUpdatedNow();
        }
    }
}
