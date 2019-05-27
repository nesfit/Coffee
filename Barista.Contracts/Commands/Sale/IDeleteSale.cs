using System;

namespace Barista.Contracts.Commands.Sale
{
    public interface IDeleteSale : ICommand
    {
        Guid Id { get; }
    }
}
