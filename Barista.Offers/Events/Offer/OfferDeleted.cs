using System;
using Barista.Contracts.Events.Offer;

namespace Barista.Offers.Events.Offer
{
    public class OfferDeleted : IOfferDeleted
    {
        public OfferDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
