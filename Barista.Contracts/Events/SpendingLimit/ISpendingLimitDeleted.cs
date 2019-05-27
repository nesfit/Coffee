using System;

namespace Barista.Contracts.Events.SpendingLimit
{
    public interface ISpendingLimitDeleted : IEvent
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
    }
}
