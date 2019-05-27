using System;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public interface IUserVerifier : IExistenceVerifier<Guid>
    {
    }
}
