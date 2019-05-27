using System;

namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface IResolveAuthenticationMeansUserId : ICommand
    {
        Guid AuthenticationMeansId { get; }
        bool ExcludeSharedAuthenticationMeans { get; }
    }
}
