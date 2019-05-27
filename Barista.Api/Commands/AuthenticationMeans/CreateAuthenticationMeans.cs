using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Api.Commands.AuthenticationMeans
{
    public class CreateAuthenticationMeans : ICreateAuthenticationMeans
    {
        public CreateAuthenticationMeans(Guid id, string label, string type, string value, DateTimeOffset validSince, DateTimeOffset? validUntil)
        {
            Id = id;
            Label = label;
            Method = type;
            Value = value;
            ValidSince = validSince;
            ValidUntil = validUntil;
        }

        public Guid Id { get; }
        public string Label { get; }
        public string Method { get; }
        public string Value { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
    }
}
