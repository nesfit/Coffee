using System;
using Barista.Contracts.Commands.PointOfSaleIntegration;

namespace Barista.Api.Commands.PointOfSaleIntegration
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
