using System;

namespace Barista.Contracts.Events.SpendingLimit
{
    public interface ISpendingLimitUpdated : IEvent
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
        TimeSpan Interval { get; }
        decimal Value { get; }
    }
}
