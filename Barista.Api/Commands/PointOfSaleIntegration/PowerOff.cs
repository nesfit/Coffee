using System;
using Barista.Contracts.Commands.PointOfSaleIntegration;

namespace Barista.Api.Commands.PointOfSaleIntegration
{
    public class PowerOff : IPowerOff
    {
        public PowerOff(Guid pointOfSaleId)
        {
            PointOfSaleId = pointOfSaleId;
        }

        public Guid PointOfSaleId { get; }
    }
}
