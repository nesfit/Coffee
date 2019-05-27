using System;
using Barista.Contracts.Events.StockItem;

namespace Barista.StockItems.Events.StockItem
{
    public class StockItemCreated : IStockItemCreated
    {
        public StockItemCreated(Guid id, string displayName, Guid pointOfSaleId)
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
