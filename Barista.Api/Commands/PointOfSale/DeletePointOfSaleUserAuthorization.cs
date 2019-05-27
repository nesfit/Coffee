using System;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;

namespace Barista.Api.Commands.PointOfSale
{
    public class DeletePointOfSaleUserAuthorization : IDeletePointOfSaleUserAuthorization
    {
        public DeletePointOfSaleUserAuthorization(Guid pointOfSaleId, Guid userId)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
    }
}
