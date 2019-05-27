using System;
using Barista.Contracts;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class GetAssignment : IQuery<AssignmentDto>
    {
        public Guid Id { get; }

        public GetAssignment(Guid id)
        {
            Id = id;
        }
    }
}
