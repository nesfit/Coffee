using System;
using System.Collections.Generic;
using System.Linq;

namespace Barista.Identity.Domain
{
    public class AssignmentToUser : Assignment
    {
        public AssignmentToUser(Guid id, Guid meansId, DateTimeOffset validSince, DateTimeOffset? validUntil, Guid userId, bool isShared) : base(id, meansId, validSince, validUntil)
        {
            SetUserId(userId);
            SetIsShared(isShared);
        }

        public Guid UserId { get; protected set; }
        public bool IsShared { get; protected set; }
        public virtual ICollection<SpendingLimit> SpendingLimits { get; protected set; } = new List<SpendingLimit>();

        public void SetUserId(Guid userId)
        {
            UserId = userId;
            SetUpdatedNow();
        }

        public void SetIsShared(bool isShared)
        {
            IsShared = isShared;
            SetUpdatedNow();
        }
    }
}
