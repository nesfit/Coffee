using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleIntegration;
using Barista.Mqtt.Messages.PointOfSale;
using Barista.Mqtt.TopicClassifiers;

namespace Barista.Mqtt.Handlers.PointOfSaleIntegration
{
    public class PowerOffCommandHandler : ICommandHandler<IPowerOff, IOperationResult>
    {
        private readonly IMqttMessagePublisher _messagePublisher;
        private readonly IPosFeatureVerifier _featureVerifier;
        private readonly IPosTopicClassifier _posTopicClassifier;

        public PowerOffCommandHandler(IMqttMessagePublisher messagePublisher, IPosFeatureVerifier featureVerifier, IPosTopicClassifier posTopicClassifier)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
            _featureVerifier = featureVerifier ?? throw new ArgumentNullException(nameof(featureVerifier));
            _posTopicClassifier = posTopicClassifier ?? throw new ArgumentNullException(nameof(posTopicClassifier));
        }

        public async Task<IOperationResult> HandleAsync(IPowerOff command, ICorrelationContext correlationContext)
        {
            await _featureVerifier.AssertFeature(command.PointOfSaleId, PosFeatures.PowerStateManagement);
            await _messagePublisher.Publish(_posTopicClassifier.GetCommandsTopic(command.PointOfSaleId), new SetPowerOff());
            return OperationResult.Ok();
        }
    }
}
