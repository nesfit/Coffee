using System;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.Api.Commands.AccountingGroup
{
    public class UpdateAccountingGroup : IUpdateAccountingGroup
    {
        public UpdateAccountingGroup(Guid id, string displayName, Guid saleStrategyId)
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
