using System;

namespace Barista.Contracts.Commands.SpendingLimit
{
    public interface IDeleteSpendingLimit : ICommand
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
    }
}
