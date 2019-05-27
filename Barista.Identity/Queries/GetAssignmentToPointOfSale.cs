using System;
using Barista.Contracts;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class GetAssignmentToPointOfSale : IQuery<AssignmentToPointOfSaleDto>
    {
        public Guid Id { get; }

        public GetAssignmentToPointOfSale(Guid id)
        {
            Id = id;
        }
    }
}
