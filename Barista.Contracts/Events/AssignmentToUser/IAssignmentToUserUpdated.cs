using System;

namespace Barista.Contracts.Events.AssignmentToUser
{
    public interface IAssignmentToUserUpdated : IEvent
    {
        Guid Id { get; }
        Guid MeansId { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
        Guid UserId { get; }
        bool IsShared { get; }
    }
}
