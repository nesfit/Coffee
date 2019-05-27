using System;

namespace Barista.Contracts.Events.AssignmentToPointOfSale
{
    public interface IAssignmentToPointOfSaleUpdated : IEvent
    {
        Guid Id { get; }
        Guid MeansId { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
        Guid PointOfSaleId { get; }
    }
}
