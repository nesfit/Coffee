using System;

namespace Barista.Contracts.Commands.Swipe
{
    public interface ICancelSwipe : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid SaleId { get; }
    }
}
