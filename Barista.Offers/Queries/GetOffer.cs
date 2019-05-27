using System;
using Barista.Common;
using Barista.Contracts;
using Barista.Offers.Dto;

namespace Barista.Offers.Queries
{
    public class GetOffer : IQuery<OfferDto>
    {
        public GetOffer(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
