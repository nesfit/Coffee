using System;

namespace Barista.Contracts.Events.StockItem
{
    public interface IStockItemDeleted : IEvent
    {
        Guid Id { get; }
    }
}
