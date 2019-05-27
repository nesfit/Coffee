using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Api.Commands.AuthenticationMeans
{
    public class UpdateAuthenticationMeans : IUpdateAuthenticationMeans
    {
        public UpdateAuthenticationMeans(Guid id, string label, string type, DateTimeOffset validSince, DateTimeOffset? validUntil)
        {
            Id = id;
            Label = label;
            Type = type;
            ValidSince = validSince;
            ValidUntil = validUntil;
        }

        public Guid Id { get; }
        public string Label { get; }
        public string Type { get; }
        public DateTimeOffset ValidSince { get; }
        public DateTimeOffset? ValidUntil { get; }
    }
}
