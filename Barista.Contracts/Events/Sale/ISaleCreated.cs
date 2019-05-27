using System;

namespace Barista.Contracts.Events.Sale
{
    public interface ISaleCreated : IEvent
    {
        Guid Id { get; }
        decimal Cost { get; }
        decimal Quantity { get; }
        Guid AccountingGroupId { get; }
        Guid UserId { get; }
        Guid AuthenticationMeansId { get; }
        Guid PointOfSaleId { get; }
        Guid ProductId { get; }
        Guid OfferId { get; }
    }
}
