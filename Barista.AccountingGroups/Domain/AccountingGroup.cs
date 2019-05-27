using System;
using Barista.Common;

namespace Barista.AccountingGroups.Domain
{
    public class AccountingGroup : Entity
    {
        public string DisplayName { get; private set; }
        public Guid SaleStrategyId { get; private set; }

        public AccountingGroup(Guid id, string displayName, Guid saleStrategyId) : base(id)
        {
            SetDisplayName(displayName);
            SetSaleStrategyId(saleStrategyId);
        }

        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new BaristaException("invalid_display_name", "Display name cannot be empty");

            DisplayName = displayName;
            SetUpdatedNow();
        }

        public void SetSaleStrategyId(Guid saleStrategyId)
        {
            SaleStrategyId = saleStrategyId;
            SetUpdatedNow();
        }
    }
}
