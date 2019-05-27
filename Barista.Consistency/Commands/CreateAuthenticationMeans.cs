using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Consistency.Commands
{
    public class CreateAuthenticationMeans : ICreateAuthenticationMeans
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Method { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ValidSince { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
    }
}
