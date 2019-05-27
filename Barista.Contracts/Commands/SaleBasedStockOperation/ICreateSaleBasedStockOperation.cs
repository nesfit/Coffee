using System;

namespace Barista.Contracts.Commands.SaleBasedStockOperation
{
    public interface ICreateSaleBasedStockOperation : ICommand
    {
        Guid Id { get; }
        Guid StockItemId { get; }
        decimal Quantity { get; }
        Guid SaleId { get; }
    }
}
