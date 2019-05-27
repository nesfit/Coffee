using System;

namespace Barista.StockOperations.Domain
{
    public class ManualStockOperation : StockOperation
    {
        public Guid CreatedByUserId { get; private set; }
        public string Comment { get; private set; }

        public ManualStockOperation(Guid id, Guid stockItemId, decimal quantity, Guid createdByUserId, string comment) :
            base(id, stockItemId, quantity)
        {
            SetCreatedByUserId(createdByUserId);
            SetComment(comment);
        }

        public void SetCreatedByUserId(Guid createdByUserId)
        {
            CreatedByUserId = createdByUserId;
            SetUpdatedNow();
        }

        public void SetComment(string comment)
        {
            Comment = comment;
            SetUpdatedNow();            
        }
    }
}
