using System;

namespace Barista.StockOperations.Dto
{
    public class SaleBasedStockOperationDto
    {
        public Guid Id { get; set; }
        public Guid StockItemId { get; set; }
        public decimal Quantity { get; set; }
        public Guid SaleId { get; set; }
    }
}
