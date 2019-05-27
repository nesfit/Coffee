using System;
using Barista.Contracts.Commands.Operations;

namespace Barista.Api.Commands.Operations
{
    public class HandleCreationOfPointOfSale : IHandleCreationOfPointOfSale
    {
        public HandleCreationOfPointOfSale(Guid id, string displayName, Guid parentAccountingGroupId, Guid? saleStrategyId, string[] features, Guid ownerUserId)
        {
            Id = id;
            DisplayName = displayName;
            ParentAccountingGroupId = parentAccountingGroupId;
            SaleStrategyId = saleStrategyId;
            Features = features ?? new string[0];
            OwnerUserId = ownerUserId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid ParentAccountingGroupId { get; }
        public Guid? SaleStrategyId { get; }
        public string[] Features { get; }
        public Guid OwnerUserId { get; }
    }
}
