using System;
using Barista.Contracts.Commands.PointOfSaleIntegration;

namespace Barista.Api.Commands.PointOfSaleIntegration
{
    public class PowerOn : IPowerOn
    {
        public PowerOn(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
        }

        public Guid PointOfSaleId { get; }
    }
}
