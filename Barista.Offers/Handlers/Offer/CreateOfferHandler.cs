using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Offer;
using Barista.Offers.Events.Offer;
using Barista.Offers.Repositories;
using Barista.Offers.Verifiers;

namespace Barista.Offers.Handlers.Offer
{
    public class CreateOfferHandler : ICommandHandler<ICreateOffer, IIdentifierResult>
    {
        private readonly IOfferRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IProductVerifier _productVerifier;
        private readonly IStockItemVerifier _stockItemVerifier;

        public CreateOfferHandler(IOfferRepository repository, IBusPublisher busPublisher, IPointOfSaleVerifier posVerifier, IProductVerifier productVerifier, IStockItemVerifier stockItemVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));

            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _productVerifier = productVerifier ?? throw new ArgumentNullException(nameof(productVerifier));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateOffer command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await Task.WhenAll(
                _posVerifier.AssertExists(command.PointOfSaleId),
                _productVerifier.AssertExists(command.ProductId),
                command.StockItemId != null ? _stockItemVerifier.AssertExists(command.StockItemId.Value) : Task.CompletedTask
            );

            var offer = new Domain.Offer(command.Id, command.PointOfSaleId, command.ProductId, command.RecommendedPrice, command.StockItemId, command.ValidSince, command.ValidUntil);
            await _repository.AddAsync(offer);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("offer_already_exists", $"An offer with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new OfferCreated(offer.Id, offer.PointOfSaleId, offer.ProductId,
                offer.RecommendedPrice, offer.StockItemId, offer.ValidSince, offer.ValidUntil));

            return new IdentifierResult(offer.Id);
        }
    }
}
