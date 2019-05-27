using System;
using System.Collections.Generic;

namespace Barista.Api.Models.Identity
{
    public class AssignmentToUser : Assignment
    {
        public bool IsShared { get; set; }
        public Guid AssignedToUserId { get; set; }
        public List<SpendingLimit> SpendingLimits { get; set; }
    }
}
