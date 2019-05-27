using System;
using Barista.Contracts.Commands.SaleBasedStockOperation;

namespace Barista.Consistency.Commands
{
    public class DeleteSaleBasedStockOperation : IDeleteSaleBasedStockOperation
    {
        public DeleteSaleBasedStockOperation(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
