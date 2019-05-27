using System;

namespace Barista.Contracts.Events.SaleStateChange
{
    public interface ISaleStateChangeCreated : IEvent
    {
        Guid Id { get; }
        Guid SaleId { get; }
        DateTimeOffset Created { get; }
        string Reason { get; }
        string State { get; }
        Guid? CausedByPointOfSaleId { get; }
        Guid? CausedByUserId { get; }
    }
}
