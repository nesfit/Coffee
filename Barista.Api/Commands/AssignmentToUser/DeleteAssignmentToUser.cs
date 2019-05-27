using System;
using Barista.Contracts.Commands.AssignmentToUser;

namespace Barista.Api.Commands.AssignmentToUser
{
    public class DeleteAssignmentToUser : IDeleteAssignmentToUser
    {
        public DeleteAssignmentToUser(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
