using System;

namespace Barista.Contracts.Events.AssignmentToUser
{
    public interface IAssignmentToUserDeleted : IEvent
    {
        Guid Id { get; }
    }
}
