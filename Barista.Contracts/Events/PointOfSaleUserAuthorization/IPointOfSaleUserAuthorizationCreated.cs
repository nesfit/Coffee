using System;

namespace Barista.Contracts.Events.PointOfSaleUserAuthorization
{
    public interface IPointOfSaleUserAuthorizationCreated : IEvent
    {
        Guid PointOfSaleId { get; }
        Guid UserId { get; }
        string Level { get; }
    }
}
