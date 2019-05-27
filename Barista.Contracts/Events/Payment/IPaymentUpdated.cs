using System;

namespace Barista.Contracts.Events.Payment
{
    public interface IPaymentUpdated : IEvent
    {
        Guid Id { get; }
        decimal Amount { get; }
        Guid UserId { get; }
        string Source { get; }
        string ExternalId { get; }
    }
}
