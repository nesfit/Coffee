using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Offer;
using Barista.Offers.Events.Offer;
using Barista.Offers.Repositories;
using Barista.Offers.Verifiers;

namespace Barista.Offers.Handlers.Offer
{
    public class UpdateOfferHandler : ICommandHandler<IUpdateOffer, IOperationResult>
    {
        private readonly IOfferRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IProductVerifier _productVerifier;
        private readonly IStockItemVerifier _stockItemVerifier;

        public UpdateOfferHandler(IOfferRepository repository, IBusPublisher busPublisher, IPointOfSaleVerifier posVerifier, IProductVerifier productVerifier, IStockItemVerifier stockItemVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));

            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _productVerifier = productVerifier ?? throw new ArgumentNullException(nameof(productVerifier));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateOffer command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var offer = await _repository.GetAsync(command.Id);
            if (offer is null)
                throw new BaristaException("offer_not_found", $"Could not find offer with ID '{command.Id}'");

            await Task.WhenAll(
                _posVerifier.AssertExists(command.PointOfSaleId),
                _productVerifier.AssertExists(command.ProductId),
                command.StockItemId != null ? _stockItemVerifier.AssertExists(command.StockItemId.Value) : Task.CompletedTask
            );

            offer.SetPointOfSaleId(command.PointOfSaleId);
            offer.SetProductId(command.ProductId);
            offer.SetRecommendedPrice(command.RecommendedPrice);
            offer.SetStockItemId(command.StockItemId);
            offer.SetValidity(command.ValidSince, command.ValidUntil);

            await _repository.UpdateAsync(offer);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new OfferUpdated(offer.Id, offer.PointOfSaleId, offer.ProductId,
                offer.RecommendedPrice, offer.StockItemId, offer.ValidSince, offer.ValidUntil));

            return OperationResult.Ok();
        }
    }
}
