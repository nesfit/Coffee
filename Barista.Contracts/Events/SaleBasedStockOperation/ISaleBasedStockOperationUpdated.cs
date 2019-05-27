using System;

namespace Barista.Contracts.Events.SaleBasedStockOperation
{
    public interface ISaleBasedStockOperationUpdated : IEvent
    {
        Guid Id { get; }
        Guid StockItemId { get; }
        decimal Quantity { get; }
        Guid SaleId { get; }
    }
}
