using System;

namespace Barista.Contracts.Commands.PointOfSaleIntegration
{
    public interface IPowerOff : ICommand
    {
        Guid PointOfSaleId { get; }
    }
}
