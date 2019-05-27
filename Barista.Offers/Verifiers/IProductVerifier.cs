using System;
using Barista.Common;

namespace Barista.Offers.Verifiers
{
    public interface IProductVerifier : IExistenceVerifier<Guid>
    {
    }
}
