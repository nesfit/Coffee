using System;

namespace Barista.Api.Models.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public decimal? RecommendedPrice { get; set; }
    }
}
