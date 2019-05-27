using System;
using Barista.Contracts.Commands.SpendingLimit;

namespace Barista.Api.Commands.SpendingLimits
{
    public class UpdateSpendingLimit : IUpdateSpendingLimit
    {
        public UpdateSpendingLimit(Guid id, Guid parentUserAssignmentId, TimeSpan interval, decimal value)
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
