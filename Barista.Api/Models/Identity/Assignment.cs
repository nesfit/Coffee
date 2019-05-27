using System;

namespace Barista.Api.Models.Identity
{
    public class Assignment
    {
        public Guid Id { get; set; }
        public Guid Means { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}
