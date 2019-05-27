using System;
using Barista.Contracts.Events.AccountingGroupUserAuthorization;

namespace Barista.AccountingGroups.Events.UserAuthorization
{
    public class UserAuthorizationDeleted : IAccountingGroupUserAuthorizationDeleted
    {
        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }

        public UserAuthorizationDeleted(Guid accountingGroupId, Guid userId)
        {
            AccountingGroupId = accountingGroupId;
            UserId = userId;
        }
    }
}
