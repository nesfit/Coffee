using System;

namespace Barista.Contracts.Commands.Operations
{
    public interface IHandleCreationOfPointOfSale : PointOfSale.ICreatePointOfSale, ICommand
    {
        Guid OwnerUserId { get; }
    }
}
