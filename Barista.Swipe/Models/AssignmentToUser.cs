using System;
using System.Collections.Generic;

namespace Barista.Swipe.Models
{
    public class AssignmentToUser
    {
        public Guid Id { get; set; }
        public Guid Means { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public bool IsShared { get; set; }
        public Guid AssignedToUserId { get; set; }
        public List<SpendingLimit> SpendingLimits { get; set; }
    }
}
