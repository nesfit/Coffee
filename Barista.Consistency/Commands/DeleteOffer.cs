using System;
using Barista.Contracts.Commands.Offer;

namespace Barista.Consistency.Commands
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
