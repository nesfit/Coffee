using System;
using Barista.Common;

namespace Barista.Offers.Verifiers
{
    public interface IStockItemVerifier : IExistenceVerifier<Guid>
    {
    }
}
