using System;
using System.Threading.Tasks;
using Barista.Accounting.Events.Sale;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Sale;

namespace Barista.Accounting.Handlers.Sale
{
    public class UpdateSaleHandler : ICommandHandler<IUpdateSale, IOperationResult>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAccountingGroupVerifier _agVerifier;
        private readonly IUserVerifier _userVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IAuthenticationMeansVerifier _amVerifier;
        private readonly IProductVerifier _productVerifier;
        private readonly IOfferVerifier _offerVerifier;

        public UpdateSaleHandler(ISalesRepository salesRepository, IBusPublisher busPublisher, IAccountingGroupVerifier agVerifier, IUserVerifier userVerifier, IPointOfSaleVerifier posVerifier, IAuthenticationMeansVerifier amVerifier, IProductVerifier productVerifier, IOfferVerifier offerVerifier)
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

        public async Task<IOperationResult> HandleAsync(IUpdateSale command, ICorrelationContext context)
        {
            var sale = await _salesRepository.GetAsync(command.Id);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Sale with ID '{command.Id}' was not found");

            await Task.WhenAll(
                _agVerifier.AssertExists(command.AccountingGroupId),
                _userVerifier.AssertExists(command.UserId),
                _posVerifier.AssertExists(command.PointOfSaleId),
                _amVerifier.AssertExists(command.AuthenticationMeansId),
                _productVerifier.AssertExists(command.ProductId),
                _offerVerifier.AssertExists(command.OfferId)
            );

            sale.SetAuthenticationMeansId(command.AuthenticationMeansId);
            sale.SetAccountingGroupId(command.AccountingGroupId);
            sale.SetCost(command.Cost);
            sale.SetPointOfSaleId(command.PointOfSaleId);
            sale.SetQuantity(command.Quantity);
            sale.SetUserId(command.UserId);
            sale.SetProductId(command.ProductId);
            sale.SetOfferId(command.OfferId);

            await _salesRepository.UpdateAsync(sale);
            await _salesRepository.SaveChanges();
            await _busPublisher.Publish(new SaleUpdated(sale));

            return OperationResult.Ok();
        }
    }
}
