using System;

namespace Barista.Contracts.Events.ManualStockOperation
{
    public interface IManualStockOperationCreated : IEvent
    {
        Guid Id { get; }
        Guid StockItemId { get; }
        decimal Quantity { get; }
        Guid CreatedByUserId { get; }
        string Comment { get; }
    }
}
