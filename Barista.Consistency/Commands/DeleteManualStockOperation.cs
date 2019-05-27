using System;
using Barista.Contracts.Commands.ManualStockOperation;

namespace Barista.Consistency.Commands
{
    public class DeleteManualStockOperation : IDeleteManualStockOperation
    {
        public DeleteManualStockOperation(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
