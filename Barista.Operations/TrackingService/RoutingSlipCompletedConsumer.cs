using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Barista.Operations.TrackingService
{
    public class RoutingSlipCompletedConsumer : IConsumer<RoutingSlipCompleted>
    {
        readonly RoutingSlipMetrics _metrics;

        public RoutingSlipCompletedConsumer(RoutingSlipMetrics metrics)
        {
            _metrics = metrics;
        }

        public async Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            _metrics.AddComplete(context.Message.Duration);
        }
    }
}
