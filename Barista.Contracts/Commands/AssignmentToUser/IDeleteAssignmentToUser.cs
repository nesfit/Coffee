using System;

namespace Barista.Contracts.Commands.AssignmentToUser
{
    public interface IDeleteAssignmentToUser : ICommand
    {
        Guid Id { get; }
    }
}
