using System;

namespace Barista.Api.Models.StockOperations
{
    public class ManualStockOperation : StockOperation
    {
        public Guid CreatedByUserId { get; set; }
        public string Comment { get; set; }
    }
}
