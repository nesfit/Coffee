using System;

namespace Barista.Identity.Dto
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public Guid Means { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}
