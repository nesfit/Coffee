using System;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;

namespace Barista.Operations.Commands.PointOfSaleUserAuthorization
{
    public class CreatePointOfSaleOwnership : ICreatePointOfSaleUserAuthorization
    {
        public CreatePointOfSaleOwnership(Guid pointOfSaleId, Guid userId)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
        public string Level => "Owner";
    }
}
