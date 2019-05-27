using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Identity.Commands.AuthenticationMeans
{
    public class UpdateAuthenticationMeansValue : IUpdateAuthenticationMeansValue
    {
        public UpdateAuthenticationMeansValue(Guid id, string value)
        {
            Id = id;
            Value = value;
        }

        public Guid Id { get; }
        public string Value { get; }
    }
}
