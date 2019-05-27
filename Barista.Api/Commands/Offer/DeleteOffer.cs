using System;
using Barista.Contracts.Commands.Offer;

namespace Barista.Api.Commands.Offer
{
    public class DeleteOffer : IDeleteOffer
    {
        public DeleteOffer(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
