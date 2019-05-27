using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Events.Sale;
using Barista.Accounting.Events.SaleStateChange;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Sale;

namespace Barista.Accounting.Handlers.Sale
{
    public class CreateSaleHandler : ICommandHandler<ICreateSale, IIdentifierResult>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAccountingGroupVerifier _agVerifier;
        private readonly IUserVerifier _userVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IAuthenticationMeansVerifier _amVerifier;
        private readonly IProductVerifier _productVerifier;
        private readonly IOfferVerifier _offerVerifier;

        public CreateSaleHandler(ISalesRepository salesRepository, IBusPublisher busPublisher, IAccountingGroupVerifier agVerifier, IUserVerifier userVerifier, IPointOfSaleVerifier posVerifier, IAuthenticationMeansVerifier amVerifier, IProductVerifier productVerifier, IOfferVerifier offerVerifier)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _agVerifier = agVerifier ?? throw new ArgumentNullException(nameof(agVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _amVerifier = amVerifier ?? throw new ArgumentNullException(nameof(amVerifier));
            _productVerifier = productVerifier ?? throw new ArgumentNullException(nameof(productVerifier));
            _offerVerifier = offerVerifier ?? throw new ArgumentNullException(nameof(offerVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateSale command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _agVerifier.AssertExists(command.AccountingGroupId),
                _userVerifier.AssertExists(command.UserId),
                _posVerifier.AssertExists(command.PointOfSaleId),
                _amVerifier.AssertExists(command.AuthenticationMeansId),
                _productVerifier.AssertExists(command.ProductId),
                _offerVerifier.AssertExists(command.OfferId)
            );

            var sale = new Domain.Sale(command.Id, command.Cost, command.Quantity, command.AccountingGroupId, command.UserId, command.AuthenticationMeansId, command.PointOfSaleId, command.ProductId, command.OfferId);
            var initialSaleStateChange = new Domain.SaleStateChange(Guid.NewGuid(), "Created", default(SaleState), command.PointOfSaleId, null);
            sale.AddStateChange(initialSaleStateChange);

            await _salesRepository.AddAsync(sale);

            try
            {
                await _salesRepository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("sale_already_exists", $"A sale with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new SaleCreated(sale));
            await _busPublisher.Publish(new SaleStateChangeCreated(initialSaleStateChange.Id, sale.Id,
                initialSaleStateChange.Created, initialSaleStateChange.Reason, initialSaleStateChange.State.ToString(),
                initialSaleStateChange.CausedByPointOfSaleId, initialSaleStateChange.CausedByUserId));
            return new IdentifierResult(sale.Id);
        }
    }
}
