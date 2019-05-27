using System;
using Barista.Contracts.Events.Product;

namespace Barista.Products.Events
{
    public class ProductCreated : IProductCreated
    {
        public ProductCreated(Guid id, string displayName, decimal? recommendedPrice)
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
