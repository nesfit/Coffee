using System;
using Barista.Contracts.Events.SpendingLimit;

namespace Barista.Identity.Events.SpendingLimit
{
    public class SpendingLimitDeleted : ISpendingLimitDeleted
    {
        public SpendingLimitDeleted(Guid id, Guid parentUserAssignmentId)
        {
            Id = id;
            ParentUserAssignmentId = parentUserAssignmentId;
        }

        public Guid Id { get; }
        public Guid ParentUserAssignmentId { get; }
    }
}
