using System;
using Barista.Contracts;

namespace Barista.StockOperations.Queries
{
    public class GetStockItemBalance : IQuery<decimal>
    {
        public Guid StockItemId { get; set; }
    }
}
