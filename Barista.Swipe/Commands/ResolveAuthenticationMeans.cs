using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Swipe.Commands
{
    public class ResolveAuthenticationMeans : IResolveAuthenticationMeans
    {
        public ResolveAuthenticationMeans(string method, string value)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(method));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            Method = method;
            Value = value;
        }

        public string Method { get; }
        public string Value { get; }
    }
}
