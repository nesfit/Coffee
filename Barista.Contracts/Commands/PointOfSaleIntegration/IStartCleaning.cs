using System;

namespace Barista.Contracts.Commands.PointOfSaleIntegration
{
    public interface IStartCleaning : ICommand
    {
        Guid PointOfSaleId { get; }
    }
}
