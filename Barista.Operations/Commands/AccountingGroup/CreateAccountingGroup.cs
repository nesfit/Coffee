using System;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.Operations.Commands.AccountingGroup
{
    public class CreateAccountingGroup : ICreateAccountingGroup
    {
        public CreateAccountingGroup(Guid id, string displayName, Guid saleStrategyId)
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
