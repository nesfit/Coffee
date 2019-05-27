using System;

namespace Barista.Swipe.Models
{
    public class PointOfSale
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid ParentAccountingGroupId { get; set; }
        public Guid? SaleStrategyId { get; set; }
    }
}
