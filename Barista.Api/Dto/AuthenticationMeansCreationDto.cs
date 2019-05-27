using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class AuthenticationMeansCreationDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        public string Label { get; set; }

        public DateTimeOffset ValidSince { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ValidUntil { get; set; }
    }
}
