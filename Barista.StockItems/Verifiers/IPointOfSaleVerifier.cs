using System;
using Barista.Common;

namespace Barista.StockItems.Verifiers
{
    public interface IPointOfSaleVerifier : IExistenceVerifier<Guid>
    {
    }
}
