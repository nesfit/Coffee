using System;
using Barista.Contracts.Commands.Swipe;

namespace Barista.Mqtt.Commands
{
    public class ProcessSwipe : IProcessSwipe
    {
        public ProcessSwipe(string authenticationMeansMethod, string authenticationMeansValue, Guid pointOfSaleId, Guid offerId, decimal quantity)
        {
            AuthenticationMeansMethod = authenticationMeansMethod;
            AuthenticationMeansValue = authenticationMeansValue;
            PointOfSaleId = pointOfSaleId;
            OfferId = offerId;
            Quantity = quantity;
        }

        public string AuthenticationMeansMethod { get; }
        public string AuthenticationMeansValue { get; }
        public Guid PointOfSaleId { get; }
        public Guid OfferId { get; }
        public decimal Quantity { get; }
    }
}
