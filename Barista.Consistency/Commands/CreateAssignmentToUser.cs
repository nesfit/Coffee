using System;
using Barista.Contracts.Commands.AssignmentToUser;

namespace Barista.Consistency.Commands
{
    public class CreateAssignmentToUser : ICreateAssignmentToUser
    {
        public Guid Id { get; set; }
        public Guid MeansId { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public Guid UserId { get; set; }
        public bool IsShared { get; set; }
    }
}
