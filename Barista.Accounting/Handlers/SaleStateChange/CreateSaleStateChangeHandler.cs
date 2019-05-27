using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Events.SaleStateChange;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SaleStateChange;

namespace Barista.Accounting.Handlers.SaleStateChange
{
    public class CreateSaleStateChangeHandler : ICommandHandler<ICreateSaleStateChange, IIdentifierResult>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBusPublisher _busPublisher;

        private readonly IUserVerifier _userVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;

        public CreateSaleStateChangeHandler(ISalesRepository salesRepository, IBusPublisher busPublisher, IUserVerifier userVerifier, IPointOfSaleVerifier posVerifier)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateSaleStateChange command, ICorrelationContext context)
        {
            var sale = await _salesRepository.GetAsync(command.ParentSaleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Sale with ID '{command.ParentSaleId}' was not found");

            if (command.CausedByUserId != null)
                await _userVerifier.AssertExists(command.CausedByUserId.Value);

            if (command.CausedByPointOfSaleId != null)
                await _posVerifier.AssertExists(command.CausedByPointOfSaleId.Value);

            var saleStateChange = new Domain.SaleStateChange(command.Id, command.Reason, Enum.Parse<SaleState>(command.State), command.CausedByPointOfSaleId, command.CausedByUserId);
            sale.AddStateChange(saleStateChange);

            await _salesRepository.UpdateAsync(sale);
            await _salesRepository.SaveChanges();

            await _busPublisher.Publish(new SaleStateChangeCreated(saleStateChange.Id, sale.Id, saleStateChange.Created,
                saleStateChange.Reason, saleStateChange.State.ToString(), saleStateChange.CausedByPointOfSaleId,
                saleStateChange.CausedByUserId));

            return new IdentifierResult(sale.Id);
        }
    }
}
