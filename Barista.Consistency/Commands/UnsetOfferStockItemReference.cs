using System;
using Barista.Contracts.Commands.Offer;

namespace Barista.Consistency.Commands
{
    public class UnsetOfferStockItemReference : IUnsetOfferStockItemReference
    {
        public UnsetOfferStockItemReference(Guid id, Guid stockItemIdToUnset)
        {
            Id = id;
            StockItemIdToUnset = stockItemIdToUnset;
        }

        public Guid Id { get; }
        public Guid StockItemIdToUnset { get; }
    }
}
