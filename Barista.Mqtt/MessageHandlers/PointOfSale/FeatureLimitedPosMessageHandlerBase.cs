using System;
using System.Threading.Tasks;
using Barista.Mqtt.TopicClassifiers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Barista.Mqtt.MessageHandlers.PointOfSale
{
    public abstract class FeatureLimitedPosMessageHandlerBase : IMqttMessageHandler
    {
        protected readonly IPosTopicClassifier PosTopicClassifier;
        protected readonly IPosFeatureVerifier FeatureVerifier;
        protected readonly ILogger Logger;

        protected FeatureLimitedPosMessageHandlerBase(IPosTopicClassifier posTopicClassifier, ILogger logger, IPosFeatureVerifier featureVerifier)
        {
            PosTopicClassifier = posTopicClassifier ?? throw new ArgumentNullException(nameof(posTopicClassifier));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            FeatureVerifier = featureVerifier ?? throw new ArgumentNullException(nameof(featureVerifier));
        }

        protected abstract string FeatureName { get; }
        protected abstract string MessageKey { get; }

        public bool AppliesAsync(string topic, JObject messagePayload)
            => PosTopicClassifier.IsMatch(topic) && messagePayload.ContainsKey(MessageKey);

        protected abstract Task HandleAsync(Guid posId, JObject messagePayload);

        public async Task HandleAsync(string topic, JObject messagePayload)
        {
            if (!PosTopicClassifier.IsMatch(topic))
                throw new InvalidOperationException("Cannot handle non-matching topic");

            var posId = PosTopicClassifier.GetPointOfSaleId(topic);
            await FeatureVerifier.AssertFeature(posId, FeatureName);

            try
            {
                await HandleAsync(posId, messagePayload);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An exception occurred while processing MQTT message in topic {topic}", topic);
            }
        }
    }
}
