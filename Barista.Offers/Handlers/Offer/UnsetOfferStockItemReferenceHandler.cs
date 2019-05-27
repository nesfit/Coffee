using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Offer;
using Barista.Offers.Events.Offer;
using Barista.Offers.Repositories;

namespace Barista.Offers.Handlers.Offer
{
    public class UnsetOfferStockItemReferenceHandler
        : ICommandHandler<IUnsetOfferStockItemReference, IOperationResult>
    {
        private readonly IOfferRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public UnsetOfferStockItemReferenceHandler(IOfferRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUnsetOfferStockItemReference command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var offer = await _repository.GetAsync(command.Id);
            if (offer is null)
                throw new BaristaException("offer_not_found", $"Could not find offer with ID '{command.Id}'");

            if (offer.StockItemId == command.StockItemIdToUnset)
            {
                offer.SetStockItemId(null);
                await _repository.UpdateAsync(offer);
                await _repository.SaveChanges();

                await _busPublisher.Publish(new OfferUpdated(offer.Id, offer.PointOfSaleId, offer.ProductId,
                    offer.RecommendedPrice, offer.StockItemId, offer.ValidSince, offer.ValidUntil));
            }

            return OperationResult.Ok();
        }
    }
}