using System;

namespace Barista.Contracts.Commands.PointOfSaleIntegration
{
    public interface IPowerOn : ICommand
    {
        Guid PointOfSaleId { get; }
    }
}
