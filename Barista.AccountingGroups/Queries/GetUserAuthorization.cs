using System;
using Barista.AccountingGroups.Dto;
using Barista.Contracts;

namespace Barista.AccountingGroups.Queries
{
    public class GetUserAuthorization : IQuery<UserAuthorizationDto>
    {
        public GetUserAuthorization(Guid accountingGroupId, Guid userId)
        {
            AccountingGroupId = accountingGroupId;
            UserId = userId;
        }

        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }
    }
}
