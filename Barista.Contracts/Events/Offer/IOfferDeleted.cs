using System;

namespace Barista.Contracts.Events.Offer
{
    public interface IOfferDeleted : IEvent
    {
        Guid Id { get; }
    }
}
