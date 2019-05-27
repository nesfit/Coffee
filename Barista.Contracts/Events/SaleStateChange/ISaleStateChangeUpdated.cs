using System;

namespace Barista.Contracts.Events.SaleStateChange
{
    public interface ISaleStateChangeUpdated : IEvent
    {
        Guid Id { get; }
        DateTimeOffset Created { get; }
        string Reason { get; }
        string State { get; }
        Guid? CausedByPointOfSaleId { get; }
        Guid? CausedByUserId { get; }
    }
}
