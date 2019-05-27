using System;

namespace Barista.Contracts.Events.PointOfSale
{
    public interface IPointOfSaleUpdated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid ParentAccountingGroupId { get; }
        Guid? SaleStrategyId { get; }
    }
}
