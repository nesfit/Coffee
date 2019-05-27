using System;
using Barista.Common;

namespace Barista.Identity.Verifiers
{
    public interface IUserVerifier : IExistenceVerifier<Guid>
    {
    }
}
