using System;

namespace Barista.Contracts.Commands.StockItem
{
    public interface IUpdateStockItem : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid PointOfSaleId { get; }
    }
}
