using System;
using Barista.Common;

namespace Barista.Products.Domain
{
    public class Product : Entity
    {
        public string DisplayName { get; private set; }
        public decimal? RecommendedPrice { get; private set; }

        public Product(Guid id, string displayName, decimal? recommendedPrice) : base(id)
        {
            SetDisplayName(displayName);
            SetRecommendedPrice(recommendedPrice);
        }

        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new BaristaException("invalid_display_name", "Display name cannot be empty");

            DisplayName = displayName;
            SetUpdatedNow();
        }

        public void SetRecommendedPrice(decimal? recommendedPrice)
        {
            if (recommendedPrice < decimal.Zero)
                throw new BaristaException("invalid_recommended_price", "Price cannot be negative");

            RecommendedPrice = recommendedPrice;
            SetUpdatedNow();
        }
    }
}
