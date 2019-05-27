using System;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class GetSpendingLimit : IQuery<SpendingLimitDto>
    {
        public GetSpendingLimit(Guid spendingLimitId, Guid parentAssignmentToUserId)
        {
            SpendingLimitId = spendingLimitId;
            ParentAssignmentToUserId = parentAssignmentToUserId;
        }

        public Guid SpendingLimitId { get; }
        public Guid ParentAssignmentToUserId { get; }
    }
}
