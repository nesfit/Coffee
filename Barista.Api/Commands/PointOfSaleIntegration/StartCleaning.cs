using System;
using Barista.Contracts.Commands.PointOfSaleIntegration;

namespace Barista.Api.Commands.PointOfSaleIntegration
{
    public class StartCleaning : IStartCleaning
    {
        public StartCleaning(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
        }

        public Guid PointOfSaleId { get; }
    }
}
