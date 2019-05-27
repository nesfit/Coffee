using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Swipe.Commands
{
    public class ResolveAuthenticationMeansUserId : IResolveAuthenticationMeansUserId
    {
        public ResolveAuthenticationMeansUserId(Guid authenticationMeansId, bool excludeSharedAuthenticationMeans)
        {
            AuthenticationMeansId = authenticationMeansId;
            ExcludeSharedAuthenticationMeans = excludeSharedAuthenticationMeans;
        }

        public Guid AuthenticationMeansId { get; }
        public bool ExcludeSharedAuthenticationMeans { get; }
    }
}
