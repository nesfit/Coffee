using System;

namespace Barista.Swipe.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public decimal? RecommendedPrice { get; set; }
    }
}
