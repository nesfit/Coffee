using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class AuthenticationMeansDto
    {
        [Required]
        public string Type { get; set; }

        public string Label { get; set; }

        public DateTimeOffset ValidSince { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ValidUntil { get; set; }
    }
}
