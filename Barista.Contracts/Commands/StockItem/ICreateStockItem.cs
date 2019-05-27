using System;

namespace Barista.Contracts.Commands.StockItem
{
    public interface ICreateStockItem : ICommand
    {
        Guid Id { get; }
        string DisplayName { get; }
        Guid PointOfSaleId { get; }
    }
}
