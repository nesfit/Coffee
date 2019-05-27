using System;
using Barista.Contracts.Events.AuthenticationMeans;

namespace Barista.Identity.Events.AuthenticationMeans
{
    public class AuthenticationMeansUpdated : IAuthenticationMeansUpdated
    {
        public Guid Id { get; }
        public string Label { get; }
        public string Type { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }

        public AuthenticationMeansUpdated(Guid id, string label, string type, DateTimeOffset validSince, DateTimeOffset? validUntil)
        {
            Id = id;
            Label = label;
            Type = type;
            ValidSince = validSince;
            ValidUntil = validUntil;
        }
    }
}
