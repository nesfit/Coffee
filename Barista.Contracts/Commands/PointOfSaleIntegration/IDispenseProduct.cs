using System;

namespace Barista.Contracts.Commands.PointOfSaleIntegration
{
    public interface IDispenseProduct : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid ProductId { get; }
    }
}
