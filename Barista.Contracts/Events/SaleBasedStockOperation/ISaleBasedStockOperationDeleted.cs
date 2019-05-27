using System;

namespace Barista.Contracts.Events.SaleBasedStockOperation
{
    public interface ISaleBasedStockOperationDeleted : IEvent
    {
        Guid Id { get; }
    }
}
