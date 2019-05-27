using System;

namespace Barista.Contracts.Events.ManualStockOperation
{
    public interface IManualStockOperationDeleted : IEvent
    {
        Guid Id { get; }
    }
}
