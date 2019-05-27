using System;
using Newtonsoft.Json;

namespace Barista.Operations.Activities.AccountingGroup.Create
{
    public class OwnershipAssignmentParameters
    {
        [JsonConstructor]
        public OwnershipAssignmentParameters(Guid id, Guid ownerUserId)
        {
            AccountingGroupId = id;
            OwnerUserId = ownerUserId;
        }

        public Guid AccountingGroupId { get; }
        public Guid OwnerUserId { get; }
    }
}