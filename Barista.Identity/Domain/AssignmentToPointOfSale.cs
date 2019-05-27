using System;

namespace Barista.Identity.Domain
{
    public class AssignmentToPointOfSale : Assignment
    {
        public AssignmentToPointOfSale(Guid id, Guid meansId, DateTimeOffset validSince, DateTimeOffset? validUntil, Guid pointOfSaleId) : base(id, meansId, validSince, validUntil)
        {
            SetPointOfSale(pointOfSaleId);
        }

        public Guid PointOfSaleId { get; protected set; }

        public void SetPointOfSale(Guid pointOfSale)
        {
            PointOfSaleId = pointOfSale;
            SetUpdatedNow();
        }
    }
}
