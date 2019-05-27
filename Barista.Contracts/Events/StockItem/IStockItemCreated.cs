using System;

namespace Barista.Contracts.Events.StockItem
{
    public interface IStockItemCreated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid PointOfSaleId { get; }
    }
}
