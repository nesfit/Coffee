using System;

namespace Barista.Contracts.Events.AuthenticationMeans
{
    public interface IAuthenticationMeansCreated : IEvent
    {
        Guid Id { get; }
        string Label { get; }
        string Type { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
