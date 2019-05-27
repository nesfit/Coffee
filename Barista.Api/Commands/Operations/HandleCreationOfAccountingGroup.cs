using System;
using Barista.Contracts.Commands.Operations;

namespace Barista.Api.Commands.Operations
{
    public class HandleCreationOfAccountingGroup : IHandleCreationOfAccountingGroup
    {
        public HandleCreationOfAccountingGroup(Guid id, string displayName, Guid saleStrategyId, Guid ownerUserId)
        {
            Id = id;
            DisplayName = displayName;
            SaleStrategyId = saleStrategyId;
            OwnerUserId = ownerUserId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid SaleStrategyId { get; }
        public Guid OwnerUserId { get; }
    }
}
