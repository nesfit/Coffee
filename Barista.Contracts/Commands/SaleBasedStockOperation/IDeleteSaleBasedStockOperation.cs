using System;

namespace Barista.Contracts.Commands.SaleBasedStockOperation
{
    public interface IDeleteSaleBasedStockOperation : ICommand
    {
        Guid Id { get; }
    }
}
