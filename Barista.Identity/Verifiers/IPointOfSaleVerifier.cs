using System;
using Barista.Common;

namespace Barista.Identity.Verifiers
{
    public interface IPointOfSaleVerifier : IExistenceVerifier<Guid>
    {
    }
}
