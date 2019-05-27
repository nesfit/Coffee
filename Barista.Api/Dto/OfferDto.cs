using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class OfferDto
    {
        [Required]
        public Guid PointOfSaleId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public decimal? RecommendedPrice { get; set; }
        public Guid? StockItemId { get; set; }

        public DateTimeOffset? ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}
