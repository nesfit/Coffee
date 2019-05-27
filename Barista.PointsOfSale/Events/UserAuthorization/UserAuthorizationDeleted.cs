using System;
using Barista.Contracts.Events.PointOfSaleUserAuthorization;

namespace Barista.PointsOfSale.Events.UserAuthorization
{
    public class UserAuthorizationDeleted : IPointOfSaleUserAuthorizationDeleted
    {
        public UserAuthorizationDeleted(Guid pointOfSaleId, Guid userId)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
    }
}
