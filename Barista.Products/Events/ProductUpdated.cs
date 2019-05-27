using System;
using Barista.Contracts.Events.Product;

namespace Barista.Products.Events
{
    public class ProductUpdated : IProductUpdated
    {
        public ProductUpdated(Guid id, string displayName, decimal? recommendedPrice)
        {
            Id = id;
            DisplayName = displayName;
            RecommendedPrice = recommendedPrice;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public decimal? RecommendedPrice { get; }
    }
}
