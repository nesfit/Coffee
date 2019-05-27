using System;
using Barista.Contracts.Events.AccountingGroup;

namespace Barista.AccountingGroups.Events.AccountingGroup
{
    public class AccountingGroupUpdated : IAccountingGroupUpdated
    {
        public AccountingGroupUpdated(Guid id, string displayName, Guid saleStrategyId)
        {
            Id = id;
            DisplayName = displayName;
            SaleStrategyId = saleStrategyId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid SaleStrategyId { get; }
    }
}
