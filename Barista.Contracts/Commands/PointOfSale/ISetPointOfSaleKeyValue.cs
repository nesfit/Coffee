using System;

namespace Barista.Contracts.Commands.PointOfSale
{
    public interface ISetPointOfSaleKeyValue : ICommand
    {
        Guid PointOfSaleId { get; }
        string Key { get; }
        string Value { get; }
    }
}
