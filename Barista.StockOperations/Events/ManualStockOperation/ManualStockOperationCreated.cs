using System;
using Barista.Contracts.Events.ManualStockOperation;

namespace Barista.StockOperations.Events.ManualStockOperation
{
    public class ManualStockOperationCreated : IManualStockOperationCreated
    {
        public ManualStockOperationCreated(Guid id, Guid stockItemId, decimal quantity, Guid createdByUserId, string comment)
        {
            Id = id;
            StockItemId = stockItemId;
            Quantity = quantity;
            CreatedByUserId = createdByUserId;
            Comment = comment;
        }

        public Guid Id { get; }
        public Guid StockItemId { get; }
        public decimal Quantity { get; }
        public Guid CreatedByUserId { get; }
        public string Comment { get; }
    }
}
