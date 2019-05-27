using System;

namespace Barista.Contracts.Commands.StockItem
{
    public interface IDeleteStockItem : ICommand
    {
        Guid Id { get; }
    }
}
