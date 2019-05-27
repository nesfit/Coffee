using System;
using Barista.Contracts.Events.AssignmentToUser;

namespace Barista.Identity.Events.AssignmentToUser
{
    public class AssignmentToUserUpdated : IAssignmentToUserUpdated
    {
        public AssignmentToUserUpdated(Guid id, Guid meansId, DateTimeOffset validSince, DateTimeOffset? validUntil, Guid userId, bool isShared)
        {
            Id = id;
            MeansId = meansId;
            ValidSince = validSince;
            ValidUntil = validUntil;
            UserId = userId;
            IsShared = isShared;
        }

        public Guid Id { get; }
        public Guid MeansId { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
        public Guid UserId { get; }
        public bool IsShared { get; }
    }
}
