using System;

namespace Barista.Contracts.Commands.AccountingGroup
{
    public interface ICreateAccountingGroup : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid SaleStrategyId { get; }
    }
}
