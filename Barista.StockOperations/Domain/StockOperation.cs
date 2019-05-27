using System;
using Barista.Common;

namespace Barista.StockOperations.Domain
{
    public abstract class StockOperation : Entity
    {
        public Guid StockItemId { get; private set; }
        public decimal Quantity { get; private set; }

        protected StockOperation(Guid id, Guid stockItemId, decimal quantity) : base(id)
        {
            SetStockItem(stockItemId);
            SetQuantity(quantity);
        }

        public void SetStockItem(Guid stockItemId)
        {
            StockItemId = stockItemId;
            SetUpdatedNow();
        }

        public void SetQuantity(decimal quantity)
        {
            Quantity = quantity;
            SetUpdatedNow();
        }
    }
}
