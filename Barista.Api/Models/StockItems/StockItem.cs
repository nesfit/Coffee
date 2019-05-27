using System;

namespace Barista.Api.Models.StockItems
{
    public class StockItem
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid PointOfSaleId { get; set; }
    }
}
