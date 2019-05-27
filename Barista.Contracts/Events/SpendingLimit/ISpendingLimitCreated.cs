using System;

namespace Barista.Contracts.Events.SpendingLimit
{
    public interface ISpendingLimitCreated : IEvent
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
        TimeSpan Interval { get; }
        decimal Value { get; }
    }
}
