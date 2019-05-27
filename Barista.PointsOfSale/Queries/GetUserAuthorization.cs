using System;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;

namespace Barista.PointsOfSale.Queries
{
    public class GetUserAuthorization : IQuery<UserAuthorizationDto>
    {
        public GetUserAuthorization(Guid pointOfSaleId, Guid userId)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
    }
}
