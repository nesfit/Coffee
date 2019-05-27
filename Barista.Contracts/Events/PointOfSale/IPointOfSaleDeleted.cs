using System;

namespace Barista.Contracts.Events.PointOfSale
{
    public interface IPointOfSaleDeleted : IEvent
    {
        Guid Id { get; }
    }
}
