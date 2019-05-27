using System;

namespace Barista.Contracts.Events.AssignmentToPointOfSale
{
    public interface IAssignmentToPointOfSaleDeleted : IEvent
    {
        Guid Id { get; }
    }
}
