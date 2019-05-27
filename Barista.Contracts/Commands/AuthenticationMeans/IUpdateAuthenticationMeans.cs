using System;

namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface IUpdateAuthenticationMeans : ICommand
    {
        Guid Id { get; }
        string Label { get; }
        string Type { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
