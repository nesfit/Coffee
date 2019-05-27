using System;

namespace Barista.Contracts.Commands.Offer
{
    public interface IUnsetOfferStockItemReference : ICommand
    {
        Guid Id { get; }
        Guid StockItemIdToUnset { get; }
    }
}
