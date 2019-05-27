using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class ManualStockOperationDto
    {
        [Required]
        public Guid StockItemId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
