using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Swipe;
using Barista.Mqtt.Commands;
using Barista.Mqtt.Models;
using Barista.Mqtt.Queries;
using Barista.Mqtt.Services;
using Barista.Mqtt.TopicClassifiers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Barista.Mqtt.MessageHandlers.PointOfSale
{
    public class ReadCardMessageHandler : FeatureLimitedPosMessageHandlerBase
    {
        public const string AuthenticationMeansType = "MFRC522SerialNumber";

        private readonly IBusPublisher _busPublisher;
        private readonly IOffersService _offersService;

        public ReadCardMessageHandler(IBusPublisher busPublisher, IOffersService offersService,
            IPosTopicClassifier posTopicClassifier, IPosFeatureVerifier featureVerifier, ILogger<ReadCardMessageHandler> logger)
            : base(posTopicClassifier, logger, featureVerifier)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
        }

        protected override string FeatureName => PosFeatures.CardReaderSwipeInitiator;
        protected override string MessageKey => "ReadCard";

        protected override async Task HandleAsync(Guid posId, JObject messagePayload)
        {
            var cardId = messagePayload[MessageKey].Value<string>();
            var offersPage = await _offersService.BrowseOffers(new BrowseOffers { AtPointOfSaleId = posId, ValidAt = DateTime.UtcNow, ResultsPerPage = 1 });

            if (offersPage.TotalResults != 1)
            {
                Logger.LogError("The point of sale {posId} must have exactly one valid offer, else swipes cannot be processed", posId);
                return;
            }

            if (!(offersPage.Items?.SingleOrDefault() is Offer offer))
            {
                Logger.LogError("A single offer was expected in the offers result set");
                return;
            }

            var command = new ProcessSwipe(AuthenticationMeansType, cardId, posId, offer.Id, 1);
            var processResult = await _busPublisher.SendRequest<IProcessSwipe, IIdentifierResult>(command);

            if (!processResult.Successful)
            {
                Logger.LogError(processResult.ToException(), "Could not process swipe");
                return;
            }
            else if (!processResult.Id.HasValue)
            {
                Logger.LogError("Swipe process was successful but did not yield a sale identifier");
                return;
            }

            var dispenseResult = await _busPublisher.SendRequest(new Commands.DispenseProduct(posId, offer.ProductId));

            var confirmResult = await _busPublisher.SendRequest<IConfirmSwipe>(new ConfirmSwipe(posId, processResult.Id.Value));
            if (!confirmResult.Successful)
            {
                Logger.LogError(confirmResult.ToException(), "Could not confirm swipe");
                return;
            }

            Logger.LogInformation($"Successfully handled sale with ID {processResult.Id.Value}");
        }
    }
}
