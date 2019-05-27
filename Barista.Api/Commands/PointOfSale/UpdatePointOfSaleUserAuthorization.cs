using System;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;

namespace Barista.Api.Commands.PointOfSale
{
    public class UpdatePointOfSaleUserAuthorization : IUpdatePointOfSaleUserAuthorization
    {
        public UpdatePointOfSaleUserAuthorization(Guid pointOfSaleId, Guid userId, string level)
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
