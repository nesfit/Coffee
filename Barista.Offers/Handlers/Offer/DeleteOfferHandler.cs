using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Offer;
using Barista.Offers.Events.Offer;
using Barista.Offers.Repositories;

namespace Barista.Offers.Handlers.Offer
{
    public class DeleteOfferHandler : ICommandHandler<IDeleteOffer, IOperationResult>
    {
        private readonly IOfferRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteOfferHandler(IOfferRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteOffer command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var offer = await _repository.GetAsync(command.Id);
            if (offer is null)
                throw new BaristaException("offer_not_found", $"Could not find offer with ID '{command.Id}'");

            await _repository.DeleteAsync(offer);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new OfferDeleted(offer.Id));

            return OperationResult.Ok();
        }
    }
}
