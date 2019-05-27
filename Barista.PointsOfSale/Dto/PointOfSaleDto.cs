using System;
using System.Collections.Generic;

namespace Barista.PointsOfSale.Dto
{
    public class PointOfSaleDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid ParentAccountingGroupId { get; set; }
        public Guid? SaleStrategyId { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
        public IEnumerable<string> Features { get; set; }
    }
}
