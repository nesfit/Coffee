using System;
using Barista.Contracts.Events.SaleBasedStockOperation;

namespace Barista.StockOperations.Events.SaleBasedStockOperation
{
    public class SaleBasedStockOperationUpdated : ISaleBasedStockOperationUpdated
    {
        public SaleBasedStockOperationUpdated(Guid id, Guid stockItemId, decimal quantity, Guid saleId)
        {
            Id = id;
            StockItemId = stockItemId;
            Quantity = quantity;
            SaleId = saleId;
        }

        public Guid Id { get; }
        public Guid StockItemId { get; }
        public decimal Quantity { get; }
        public Guid SaleId { get; }
    }
}
