using System;

namespace Barista.Contracts.Events.PointOfSale
{
    public interface IPointOfSaleKeyValueUpdated : IEvent
    {
        Guid PointOfSaleId { get; }
        string Key { get; }
        string Value { get; }
    }
}
