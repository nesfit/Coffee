using System;
using Barista.Contracts.Events.SaleBasedStockOperation;

namespace Barista.StockOperations.Events.SaleBasedStockOperation
{
    public class SaleBasedStockOperationDeleted : ISaleBasedStockOperationDeleted
    {
        public SaleBasedStockOperationDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
