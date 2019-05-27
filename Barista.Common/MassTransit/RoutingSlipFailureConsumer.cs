using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Barista.Common.MassTransit
{
    public class RoutingSlipFailureConsumer : IConsumer<RoutingSlipCompensationFailed>, IConsumer<RoutingSlipFaulted>, IConsumer<RoutingSlipActivityCompensationFailed>, IConsumer<RoutingSlipActivityFaulted>
    {
        private readonly ILogger _logger;

        public RoutingSlipFailureConsumer(ILogger<RoutingSlipFailureConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Consume(ConsumeContext<RoutingSlipCompensationFailed> context)
        {
            _logger.LogError(new EventId(77701, nameof(RoutingSlipCompensationFailed)), JsonConvert.SerializeObject(context.Message));
            return context.ConsumeCompleted;
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            _logger.LogError(new EventId(77702, nameof(RoutingSlipFaulted)), JsonConvert.SerializeObject(context.Message));
            return context.ConsumeCompleted;
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompensationFailed> context)
        {
            _logger.LogError(new EventId(77703, nameof(RoutingSlipActivityCompensationFailed)), JsonConvert.SerializeObject(context.Message));
            return context.ConsumeCompleted;
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityFaulted> context)
        {
            _logger.LogError(new EventId(77704, nameof(RoutingSlipActivityFaulted)), JsonConvert.SerializeObject(context.Message));
            return context.ConsumeCompleted;
        }
    }
}
