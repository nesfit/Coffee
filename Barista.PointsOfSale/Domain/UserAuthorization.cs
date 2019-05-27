using System;

namespace Barista.PointsOfSale.Domain
{
    public class UserAuthorization
    {
        public Guid PointOfSaleId { get; private set; }
        public Guid UserId { get; private set; }
        public UserAuthorizationLevel Level { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset Updated { get; private set; }

        public UserAuthorization(Guid pointOfSaleId, Guid userId, UserAuthorizationLevel level)
        {
            Created = DateTimeOffset.UtcNow;
            SetPointOfSaleId(pointOfSaleId);
            SetUserId(userId);
            SetLevel(level);
        }
        
        protected void SetPointOfSaleId(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
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
