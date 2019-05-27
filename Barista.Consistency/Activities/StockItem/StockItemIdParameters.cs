using System;

namespace Barista.Consistency.Activities.StockItem
{
    public class StockItemIdParameters : ConsistencyActivityParametersBase
    {
        public Guid StockItemId { get; set; }
    }
}
