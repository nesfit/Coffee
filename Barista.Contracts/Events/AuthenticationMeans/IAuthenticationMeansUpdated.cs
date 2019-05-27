using System;

namespace Barista.Contracts.Events.AuthenticationMeans
{
    public interface IAuthenticationMeansUpdated : IEvent
    {
        Guid Id { get; }
        string Label { get; }
        string Type { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
