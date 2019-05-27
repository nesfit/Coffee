using System;
using Barista.Contracts.Events.AssignmentToPointOfSale;

namespace Barista.Identity.Events.AssignmentToPointOfSale
{
    public class AssignmentToPointOfSaleCreated : IAssignmentToPointOfSaleCreated
    {
        public AssignmentToPointOfSaleCreated(Guid id, Guid ofMeans, DateTimeOffset validSince, DateTimeOffset? validUntil, Guid assignedToPointOfSaleId)
        {
            Id = id;
            MeansId = ofMeans;
            ValidSince = validSince;
            ValidUntil = validUntil;
            PointOfSaleId = assignedToPointOfSaleId;
        }

        public Guid Id { get; }
        public Guid MeansId { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
        public Guid PointOfSaleId { get; }
    }
}
