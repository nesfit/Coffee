using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Contracts.Events.Offer
{
    public interface IOfferCreated : IEvent
    {
        Guid Id { get; }
        Guid PointOfSaleId { get; }
        Guid ProductId { get; }
        decimal? RecommendedPrice { get; }
        Guid? StockItemId { get; }
        DateTimeOffset? ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
