using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class StockItemDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public Guid PointOfSaleId { get; set; }
    }
}
