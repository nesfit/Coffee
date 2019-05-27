using System;

namespace Barista.Contracts.Commands.Swipe
{
    public interface IConfirmSwipe : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid SaleId { get; }
    }
}
