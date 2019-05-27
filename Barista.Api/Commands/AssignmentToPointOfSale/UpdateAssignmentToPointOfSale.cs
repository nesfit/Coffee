using System;
using Barista.Contracts.Commands.AssignmentToPointOfSale;

namespace Barista.Api.Commands.AssignmentToPointOfSale
{
    public class UpdateAssignmentToPointOfSale : IUpdateAssignmentToPointOfSale
    {
        public UpdateAssignmentToPointOfSale(Guid id, Guid meansId, DateTimeOffset validSince, DateTimeOffset? validUntil, Guid pointOfSaleId)
        {
            Id = id;
            MeansId = meansId;
            ValidSince = validSince;
            ValidUntil = validUntil;
            PointOfSaleId = pointOfSaleId;
        }

        public Guid Id { get; }
        public Guid MeansId { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
        public Guid PointOfSaleId { get; }
    }
}
