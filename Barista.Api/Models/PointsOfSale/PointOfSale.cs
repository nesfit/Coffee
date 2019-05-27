using System;
using System.Collections.Generic;

namespace Barista.Api.Models.PointsOfSale
{
    public class PointOfSale
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid ParentAccountingGroupId { get; set; }
        public Guid? SaleStrategyId { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
        public List<string> Features { get; set; }
    }
}
