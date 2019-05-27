using System;

namespace Barista.Contracts.Events.Payment
{
    public interface IPaymentDeleted : IEvent
    {
        Guid Id { get; }
    }
}
