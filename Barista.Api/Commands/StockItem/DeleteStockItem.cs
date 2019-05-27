using System;
using Barista.Contracts.Commands.StockItem;

namespace Barista.Api.Commands.StockItem
{
    public class DeleteStockItem : IDeleteStockItem
    {
        public DeleteStockItem(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
