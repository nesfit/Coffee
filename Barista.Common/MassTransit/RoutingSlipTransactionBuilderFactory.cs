using System;
using MassTransit;

namespace Barista.Common.MassTransit
{
    public class RoutingSlipTransactionBuilderFactory : IRoutingSlipTransactionBuilderFactory
    {
        private readonly IBusControl _busControl;

        public RoutingSlipTransactionBuilderFactory(IBusControl busControl)
        {
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        }

        public IRoutingSlipTransactionBuilder Create()
        {
            return new RoutingSlipTransactionBuilder(_busControl);
        }
    }
}
