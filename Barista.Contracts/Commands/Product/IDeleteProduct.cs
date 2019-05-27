using System;

namespace Barista.Contracts.Commands.Product
{
    public interface IDeleteProduct : ICommand
    {
        Guid Id { get; }
    }
}
