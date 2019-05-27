using System;

namespace Barista.Api.Models.Identity
{
    public class AuthenticationMeans
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Label { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public bool IsValid { get; set; }
    }
}
