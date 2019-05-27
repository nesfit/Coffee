using System;
using System.Collections.Generic;

namespace Barista.Identity.Dto
{
    public class AssignmentToUserDto
    {
        public Guid Id { get; set; }
        public Guid Means { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public bool IsShared { get; set; }
        public Guid AssignedToUserId { get; set; }
        public IEnumerable<SpendingLimitDto> SpendingLimits { get; set; }
    }
}
