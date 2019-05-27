using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class ProductDto
    {
        [Required]
        public string DisplayName { get; set; }

        public decimal? RecommendedPrice { get; set; }
    }
}
