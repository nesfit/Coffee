using System;
using Barista.Common;

namespace Barista.AccountingGroups.Verifiers
{
    public interface IUserVerifier : IExistenceVerifier<Guid>
    {
    }
}
