using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts.Commands.SaleBasedStockOperation;

namespace Barista.Consistency.Commands
{
    public class CreateSaleBasedStockOperation : ICreateSaleBasedStockOperation
    {
        public CreateSaleBasedStockOperation(Guid id, Guid stockItemId, decimal quantity, Guid saleId)
        {
            Id = id;
            StockItemId = stockItemId;
            Quantity = quantity;
            SaleId = saleId;
        }

        public Guid Id { get; }
        public Guid StockItemId { get; }
        public decimal Quantity { get; }
        public Guid SaleId { get; }
    }
}
