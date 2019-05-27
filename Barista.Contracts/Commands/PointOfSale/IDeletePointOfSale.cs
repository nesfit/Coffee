using System;

namespace Barista.Contracts.Commands.PointOfSale
{
    public interface IDeletePointOfSale : ICommand
    {
        Guid Id { get; }
    }
}
