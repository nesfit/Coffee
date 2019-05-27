using System;
using Barista.Contracts;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class GetAssignmentToUser : IQuery<AssignmentToUserDto>
    {
        public Guid Id { get; }

        public GetAssignmentToUser(Guid id)
        {
            Id = id;
        }
    }
}
