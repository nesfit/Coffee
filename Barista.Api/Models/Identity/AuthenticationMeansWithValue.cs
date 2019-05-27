using System;

namespace Barista.Api.Models.Identity
{
    public class AuthenticationMeansWithValue
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public bool IsValid { get; set; }
    }
}
