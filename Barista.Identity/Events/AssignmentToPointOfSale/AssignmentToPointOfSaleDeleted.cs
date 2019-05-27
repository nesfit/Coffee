using System;
using Barista.Contracts.Events.AssignmentToPointOfSale;

namespace Barista.Identity.Events.AssignmentToPointOfSale
{
    public class AssignmentToPointOfSaleDeleted : IAssignmentToPointOfSaleDeleted
    {
        public AssignmentToPointOfSaleDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
