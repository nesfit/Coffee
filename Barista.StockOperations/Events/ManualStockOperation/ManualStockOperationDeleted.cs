using System;
using Barista.Contracts.Events.ManualStockOperation;

namespace Barista.StockOperations.Events.ManualStockOperation
{
    public class ManualStockOperationDeleted : IManualStockOperationDeleted
    {
        public ManualStockOperationDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
