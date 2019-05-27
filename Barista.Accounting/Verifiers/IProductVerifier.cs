using System;
using Barista.Common;

namespace Barista.Accounting.Verifiers
{
    public interface IProductVerifier : IExistenceVerifier<Guid>
    {
    }
}
