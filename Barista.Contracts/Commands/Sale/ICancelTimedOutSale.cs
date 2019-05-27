using System;

namespace Barista.Contracts.Commands.Sale
{
    public interface ICancelTimedOutSale : ICommand
    {
        Guid SaleId { get; }
    }
}
