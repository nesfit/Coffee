using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Barista.Operations.TrackingService
{
    public class RoutingSlipActivityCompletedConsumer :
        IConsumer<RoutingSlipActivityCompleted>
    {
        readonly string _activityName;
        readonly RoutingSlipMetrics _metrics;

        public RoutingSlipActivityCompletedConsumer(RoutingSlipMetrics metrics, string activityName)
        {
            _metrics = metrics;
            _activityName = activityName;
        }

        public async Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
        {
            if (context.Message.ActivityName.Equals(_activityName, StringComparison.OrdinalIgnoreCase))
                _metrics.AddComplete(context.Message.Duration);
        }
    }
}
