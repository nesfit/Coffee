using System;

namespace Barista.Contracts.Events.AuthenticationMeans
{
    public interface IAuthenticationMeansDeleted : IEvent
    {
        Guid Id { get; }
    }
}
