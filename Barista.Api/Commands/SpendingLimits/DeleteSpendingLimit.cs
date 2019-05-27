using System;
using Barista.Contracts.Commands.SpendingLimit;

namespace Barista.Api.Commands.SpendingLimits
{
    public class DeleteSpendingLimit : IDeleteSpendingLimit
    {
        public DeleteSpendingLimit(Guid id, Guid parentUserAssignmentId)
        {
            Id = id;
            ParentUserAssignmentId = parentUserAssignmentId;
        }

        public Guid Id { get; }
        public Guid ParentUserAssignmentId { get; }
    }
}
