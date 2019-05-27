using System;

namespace Barista.StockOperations.Domain
{
    public class SaleBasedStockOperation : StockOperation
    {
        public Guid SaleId { get; private set; }

        public SaleBasedStockOperation(Guid id, Guid stockItemId, decimal quantity, Guid saleId) : base(id, stockItemId, quantity)
        {
            SetSaleId(saleId);
        }

        public void SetSaleId(Guid saleId)
        {
            SaleId = saleId;
            SetUpdatedNow();
        }
    }
}
