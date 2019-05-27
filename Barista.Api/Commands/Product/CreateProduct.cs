using System;
using Barista.Contracts.Commands.Product;

namespace Barista.Api.Commands.Product
{
    public class CreateProduct : ICreateProduct
    {
        public CreateProduct(Guid id, string displayName, decimal? recommendedPrice)
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
