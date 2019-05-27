using System;
using Barista.Contracts.Events.PointOfSaleUserAuthorization;

namespace Barista.PointsOfSale.Events.UserAuthorization
{
    public class UserAuthorizationCreated : IPointOfSaleUserAuthorizationCreated
    {
        public UserAuthorizationCreated(Guid pointOfSaleId, Guid userId, string level)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
            Level = level;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
        public string Level { get; }
    }
}
