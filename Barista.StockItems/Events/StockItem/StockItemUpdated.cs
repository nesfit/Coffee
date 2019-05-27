using System;
using Barista.Contracts.Events.StockItem;

namespace Barista.StockItems.Events.StockItem
{
    public class StockItemUpdated : IStockItemUpdated
    {
        public StockItemUpdated(Guid id, string displayName, Guid pointOfSaleId)
        {
            Id = id;
            DisplayName = displayName;
            PointOfSaleId = pointOfSaleId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid PointOfSaleId { get; }
    }
}
