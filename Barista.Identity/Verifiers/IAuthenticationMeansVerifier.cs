using System;
using Barista.Common;

namespace Barista.Identity.Verifiers
{
    public interface IAuthenticationMeansVerifier : IExistenceVerifier<Guid>
    {
    }
}
