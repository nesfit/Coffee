using System;

namespace Barista.Contracts.Events.PointOfSale
{
    public interface IPointOfSaleCreated : IEvent
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid ParentAccountingGroupId { get; }
        Guid? SaleStrategyId { get; }
    }
}
