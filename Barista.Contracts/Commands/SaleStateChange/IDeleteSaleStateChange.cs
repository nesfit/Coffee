using System;

namespace Barista.Contracts.Commands.SaleStateChange
{
    public interface IDeleteSaleStateChange : ICommand
    {
        Guid Id { get; }
        Guid ParentSaleId { get; }
    }
}
