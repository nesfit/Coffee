using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts.Commands.PointOfSaleIntegration;

namespace Barista.Mqtt.Commands
{
    public class DispenseProduct : IDispenseProduct
    {
        public DispenseProduct(Guid pointOfSaleId, Guid productId)
        {
            PointOfSaleId = pointOfSaleId;
            ProductId = productId;
        }

        public Guid PointOfSaleId { get; }
        public Guid ProductId { get; }
    }
}
