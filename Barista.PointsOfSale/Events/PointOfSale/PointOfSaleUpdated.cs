using System;
using Barista.Contracts.Events.PointOfSale;

namespace Barista.PointsOfSale.Events.PointOfSale
{
    public class PointOfSaleUpdated : IPointOfSaleUpdated
    {
        public PointOfSaleUpdated(Guid id, string displayName, Guid parentAccountingGroupId, Guid? saleStrategyId)
        {
            Id = id;
            DisplayName = displayName;
            ParentAccountingGroupId = parentAccountingGroupId;
            SaleStrategyId = saleStrategyId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid ParentAccountingGroupId { get; }
        public Guid? SaleStrategyId { get; }
    }
}
