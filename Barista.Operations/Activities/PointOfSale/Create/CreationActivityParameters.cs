using System;

namespace Barista.Operations.Activities.PointOfSale.Create
{
    public class CreationActivityParameters
    {
        public CreationActivityParameters(Guid id, string displayName, Guid parentAccountingGroupId, Guid? saleStrategyId, string[] features)
        {
            Id = id;
            DisplayName = displayName;
            ParentAccountingGroupId = parentAccountingGroupId;
            SaleStrategyId = saleStrategyId;
            Features = features;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid ParentAccountingGroupId { get; }
        public Guid? SaleStrategyId { get; }
        public string[] Features { get; }
    }
}
