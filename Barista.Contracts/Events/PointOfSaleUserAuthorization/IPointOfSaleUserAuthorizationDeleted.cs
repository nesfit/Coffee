using System;

namespace Barista.Contracts.Events.PointOfSaleUserAuthorization
{
    public interface IPointOfSaleUserAuthorizationDeleted : IEvent
    {
        Guid PointOfSaleId { get; }
        Guid UserId { get; }
    }
}
