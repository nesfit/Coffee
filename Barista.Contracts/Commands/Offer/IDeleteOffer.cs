using System;

namespace Barista.Contracts.Commands.Offer
{
    public interface IDeleteOffer : ICommand
    {
        Guid Id { get; }
    }
}
