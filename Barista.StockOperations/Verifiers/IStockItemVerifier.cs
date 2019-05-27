using System;
using Barista.Common;

namespace Barista.StockOperations.Verifiers
{
    public interface IStockItemVerifier : IExistenceVerifier<Guid>
    {
    }
}
