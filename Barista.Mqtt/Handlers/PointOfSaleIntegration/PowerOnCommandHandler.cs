using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleIntegration;
using Barista.Mqtt.Messages.PointOfSale;
using Barista.Mqtt.TopicClassifiers;

namespace Barista.Mqtt.Handlers.PointOfSaleIntegration
{
    public class PowerOnCommandHandler : ICommandHandler<IPowerOn, IOperationResult>
    {
        private readonly IMqttMessagePublisher _messagePublisher;
        private readonly IPosFeatureVerifier _featureVerifier;
        private readonly IPosTopicClassifier _posTopicClassifier;

        public PowerOnCommandHandler(IMqttMessagePublisher messagePublisher, IPosFeatureVerifier featureVerifier, IPosTopicClassifier posTopicClassifier)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
            _featureVerifier = featureVerifier ?? throw new ArgumentNullException(nameof(featureVerifier));
            _posTopicClassifier = posTopicClassifier ?? throw new ArgumentNullException(nameof(posTopicClassifier));
        }

        public async Task<IOperationResult> HandleAsync(IPowerOn command, ICorrelationContext correlationContext)
        {
            await _featureVerifier.AssertFeature(command.PointOfSaleId, PosFeatures.PowerStateManagement);
            await _messagePublisher.Publish(_posTopicClassifier.GetCommandsTopic(command.PointOfSaleId), new SetPowerOn());
            return OperationResult.Ok();
        }
    }
}
