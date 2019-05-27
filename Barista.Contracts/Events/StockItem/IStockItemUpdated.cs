using System;

namespace Barista.Contracts.Events.StockItem
{
    public interface IStockItemUpdated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid PointOfSaleId { get; }
    }
}
