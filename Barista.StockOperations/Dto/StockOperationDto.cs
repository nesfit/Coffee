using System;

namespace Barista.StockOperations.Dto
{
    public class StockOperationDto
    {
        public Guid Id { get; set; }
        public Guid StockItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
