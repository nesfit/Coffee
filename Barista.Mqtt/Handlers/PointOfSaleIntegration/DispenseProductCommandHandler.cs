using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleIntegration;
using Barista.Mqtt.Messages.PointOfSale;
using Barista.Mqtt.TopicClassifiers;

namespace Barista.Mqtt.Handlers.PointOfSaleIntegration
{
    public class DispenseProductCommandHandler : ICommandHandler<IDispenseProduct, IOperationResult>
    {
        private readonly IMqttMessagePublisher _messagePublisher;
        private readonly IPosFeatureVerifier _featureVerifier;
        private readonly IPosTopicClassifier _posTopicClassifier;

        public DispenseProductCommandHandler(IMqttMessagePublisher messagePublisher, IPosFeatureVerifier featureVerifier, IPosTopicClassifier posTopicClassifier)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
            _featureVerifier = featureVerifier ?? throw new ArgumentNullException(nameof(featureVerifier));
            _posTopicClassifier = posTopicClassifier ?? throw new ArgumentNullException(nameof(posTopicClassifier));
        }

        public async Task<IOperationResult> HandleAsync(IDispenseProduct command, ICorrelationContext correlationContext)
        {
            await _featureVerifier.AssertFeature(command.PointOfSaleId, PosFeatures.ProductDispenser);
            await _messagePublisher.Publish(_posTopicClassifier.GetCommandsTopic(command.PointOfSaleId), new DispenseProduct());
            return OperationResult.Ok();
        }
    }
}
