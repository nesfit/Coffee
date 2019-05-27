using System;
using Barista.Contracts.Events.AuthenticationMeans;

namespace Barista.Identity.Events.AuthenticationMeans
{
    public class AuthenticationMeansDeleted : IAuthenticationMeansDeleted
    {
        public AuthenticationMeansDeleted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
