using System;

namespace Barista.Contracts.Events.AccountingGroup
{
    public interface IAccountingGroupUpdated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid SaleStrategyId { get; }
    }
}
