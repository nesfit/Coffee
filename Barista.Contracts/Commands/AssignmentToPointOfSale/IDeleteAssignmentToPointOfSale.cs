using System;

namespace Barista.Contracts.Commands.AssignmentToPointOfSale
{
    public interface IDeleteAssignmentToPointOfSale : ICommand
    {
        Guid Id { get; }
    }
}
