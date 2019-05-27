using System;
using Barista.Contracts.Events.StockItem;

namespace Barista.StockItems.Events.StockItem
{
    public class StockItemDeleted : IStockItemDeleted
    {
        public StockItemDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
