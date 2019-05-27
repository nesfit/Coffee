using System;
using Barista.Contracts.Events.AccountingGroupUserAuthorization;

namespace Barista.AccountingGroups.Events.UserAuthorization
{
    public class UserAuthorizationUpdated : IAccountingGroupUserAuthorizationUpdated
    {
        public UserAuthorizationUpdated(Guid accountingGroupId, Guid userId, string userAuthorizationLevel)
        {
            AccountingGroupId = accountingGroupId;
            UserId = userId;
            Level = userAuthorizationLevel;
        }

        public Guid AccountingGroupId { get; }
        public Guid UserId { get; }
        public string Level { get; }
    }
}
