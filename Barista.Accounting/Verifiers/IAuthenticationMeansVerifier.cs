using System;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public interface IAuthenticationMeansVerifier : IExistenceVerifier<Guid>
    {
    }
}
