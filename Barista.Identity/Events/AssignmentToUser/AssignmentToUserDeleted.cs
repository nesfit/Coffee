using System;
using Barista.Contracts.Events.AssignmentToUser;

namespace Barista.Identity.Events.AssignmentToUser
{
    public class AssignmentToUserDeleted : IAssignmentToUserDeleted
    {
        public AssignmentToUserDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
