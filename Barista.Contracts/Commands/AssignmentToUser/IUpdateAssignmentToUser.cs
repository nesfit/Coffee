using System;

namespace Barista.Contracts.Commands.AssignmentToUser
{
    public interface IUpdateAssignmentToUser :  ICommand
    {
        Guid Id { get; }
        Guid MeansId { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
        Guid UserId { get; }
        bool IsShared { get; }
    }
}
