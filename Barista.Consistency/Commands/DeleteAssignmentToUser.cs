using System;
using Barista.Contracts.Commands.AssignmentToUser;

namespace Barista.Consistency.Commands
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
