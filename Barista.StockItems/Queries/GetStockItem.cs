using System;
using Barista.Contracts;
using Barista.StockItems.Dto;

namespace Barista.StockItems.Queries
{
    public class GetStockItem : IQuery<StockItemDto>
    {
        public GetStockItem(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
