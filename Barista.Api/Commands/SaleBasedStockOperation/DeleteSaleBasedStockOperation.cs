using System;
using Barista.Contracts.Commands.SaleBasedStockOperation;

namespace Barista.Api.Commands.SaleBasedStockOperation
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
