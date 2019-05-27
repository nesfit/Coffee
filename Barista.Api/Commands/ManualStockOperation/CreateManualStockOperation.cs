using System;
using Barista.Contracts.Commands.ManualStockOperation;

namespace Barista.Api.Commands.ManualStockOperation
{
    public class CreateManualStockOperation : ICreateManualStockOperation
    {
        public CreateManualStockOperation(Guid id, Guid stockItemId, decimal quantity, Guid createdByUserId, string comment)
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
