using System;

namespace Barista.Contracts.Events.AccountingGroup
{
    public interface IAccountingGroupCreated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid SaleStrategyId { get; }
    }
}
