using System;
using Newtonsoft.Json;

namespace Barista.Operations.Activities.PointOfSale.Create
{
    public class OwnershipAssignmentParameters
    {
        [JsonConstructor]
        public OwnershipAssignmentParameters(Guid id, Guid ownerUserId)
        {
            PointOfSaleId = id;
            OwnerUserId = ownerUserId;
        }

        public Guid PointOfSaleId { get; }
        public Guid OwnerUserId { get; }
    }
}