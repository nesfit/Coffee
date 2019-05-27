using System;

namespace Barista.Contracts.Commands.Swipe
{
    public interface IProcessSwipe : ICommand
    {
        string AuthenticationMeansMethod { get; }
        string AuthenticationMeansValue { get; }
        Guid PointOfSaleId { get; }
        Guid OfferId { get; }
        decimal Quantity { get; }
    }
}
