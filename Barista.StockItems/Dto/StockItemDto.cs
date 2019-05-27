using System;
using Barista.StockItems.Domain;

namespace Barista.StockItems.Dto
{
    public class StockItemDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid PointOfSaleId { get; set; }
    }
}
