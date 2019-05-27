using System;

namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface ICreateAuthenticationMeans : ICommand
    {
        Guid Id { get; }
        string Label { get; }
        string Method { get; }
        string Value { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
