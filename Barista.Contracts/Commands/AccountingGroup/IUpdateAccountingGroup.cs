using System;

namespace Barista.Contracts.Commands.AccountingGroup
{
    public interface IUpdateAccountingGroup : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid SaleStrategyId { get; }
    }
}
