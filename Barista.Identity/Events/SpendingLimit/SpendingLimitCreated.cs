using System;
using Barista.Contracts.Events.SpendingLimit;

namespace Barista.Identity.Events.SpendingLimit
{
    public class SpendingLimitCreated : ISpendingLimitCreated
    {
        public SpendingLimitCreated(Guid id, Guid parentUserAssignmentId, TimeSpan interval, decimal value)
        {
            Id = id;
            ParentUserAssignmentId = parentUserAssignmentId;
            Interval = interval;
            Value = value;
        }

        public Guid Id { get; }
        public Guid ParentUserAssignmentId { get; }
        public TimeSpan Interval { get; }
        public decimal Value { get; }
    }
}
