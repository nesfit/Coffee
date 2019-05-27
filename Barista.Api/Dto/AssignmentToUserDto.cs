using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class AssignmentToUserDto
    {
        [Required]
        public Guid MeansId { get; set; }

        public DateTimeOffset ValidSince { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ValidUntil { get; set; }

        public Guid UserId { get; set; } = Guid.Empty;

        public bool IsShared { get; set; }
    }
}
