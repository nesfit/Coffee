using System;

namespace Barista.Api.Models.StockOperations
{
    public class SaleBasedStockOperation : StockOperation
    {
        public Guid SaleId { get; set; }
    }
}
