using System;

namespace Barista.Api.Models.StockOperations
{
    public class StockOperation
    {
        public Guid Id { get; set; }
        public Guid StockItemId { get; set; }
        public decimal Quantity { get; set; }
    }
}
