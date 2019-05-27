using System;
using Barista.Contracts.Commands.AssignmentToPointOfSale;

namespace Barista.Api.Commands.AssignmentToPointOfSale
{
    public class DeleteAssignmentToPointOfSale : IDeleteAssignmentToPointOfSale
    {
        public DeleteAssignmentToPointOfSale(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
