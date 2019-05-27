using System;

namespace Barista.Products.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public decimal? RecommendedPrice { get; set; }
    }
}
