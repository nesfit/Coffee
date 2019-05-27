using System;
using Barista.Contracts.Commands.Swipe;

namespace Barista.Mqtt.Commands
{
    public class ConfirmSwipe : IConfirmSwipe
    {
        public ConfirmSwipe(Guid pointOfSaleId, Guid saleId)
        {
            PointOfSaleId = pointOfSaleId;
            SaleId = saleId;
        }

        public Guid PointOfSaleId { get; }
        public Guid SaleId { get; }
    }
}
