using System;

namespace Barista.AccountingGroups.Domain
{
    public class UserAuthorization
    {
        public Guid AccountingGroupId { get; private set; }
        public Guid UserId { get; private set; }
        public UserAuthorizationLevel Level { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset Updated { get; private set; }

        public UserAuthorization(Guid accountingGroupId, Guid userId, UserAuthorizationLevel level)
        {
            Created = DateTimeOffset.UtcNow;
            SetAccountingGroupId(accountingGroupId);
            SetUserId(userId);
            SetLevel(level);
        }
        
        protected void SetAccountingGroupId(Guid pointOfSaleId)
        {
            AccountingGroupId = pointOfSaleId;
            Updated = DateTimeOffset.UtcNow;Updated = DateTimeOffset.UtcNow;
        }

        protected void SetUserId(Guid userId)
        {
            UserId = userId;
            Updated = DateTimeOffset.UtcNow;
        }

        public void SetLevel(UserAuthorizationLevel level)
        {
            Level = level;
            Updated = DateTimeOffset.UtcNow;
        }
    }
}
