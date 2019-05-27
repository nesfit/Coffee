using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Mqtt.Commands;
using Barista.Mqtt.TopicClassifiers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Barista.Mqtt.MessageHandlers.PointOfSale
{
    public class ReadinessReportMessageHandler : FeatureLimitedPosMessageHandlerBase
    {
        private readonly IBusPublisher _busPublisher;

        public ReadinessReportMessageHandler(IBusPublisher busPublisher, IPosTopicClassifier posTopicClassifier, IPosFeatureVerifier featureVerifier, ILogger<ReadinessReportMessageHandler> logger)
            : base(posTopicClassifier, logger, featureVerifier)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        protected override string FeatureName => PosFeatures.ReportsReadiness;
        protected override string MessageKey => "Ready";

        protected override async Task HandleAsync(Guid posId, JObject messagePayload)
            => await _busPublisher.SendRequest(new SetPointOfSaleKeyValue(posId, MessageKey, messagePayload[MessageKey].Value<string>()));
    }
}
