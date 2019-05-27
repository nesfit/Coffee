using System;

namespace Barista.Contracts.Events.Payment
{
    public interface IPaymentCreated : IEvent
    {
        Guid Id { get; }
        decimal Amount { get; }
        Guid UserId { get; }
        string Source { get; }
        string ExternalId { get; }
    }
}
