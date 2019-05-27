using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Accounting.Events.SaleStateChange;
using Barista.Accounting.Repositories;
using Barista.Accounting.Verifiers;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SaleStateChange;

namespace Barista.Accounting.Handlers.SaleStateChange
{
    public class UpdateSaleStateChangeHandler : ICommandHandler<IUpdateSaleStateChange, IOperationResult>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBusPublisher _busPublisher;

        private readonly IUserVerifier _userVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;

        public UpdateSaleStateChangeHandler(ISalesRepository salesRepository, IBusPublisher busPublisher, IPointOfSaleVerifier posVerifier, IUserVerifier userVerifier)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateSaleStateChange command, ICorrelationContext context)
        {
            var sale = await _salesRepository.GetAsync(command.ParentSaleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Sale with ID '{command.ParentSaleId}' was not found");

            var saleStateChange = sale.StateChanges.FirstOrDefault(chg => chg.Id == command.Id);
            if (saleStateChange is null)
                throw new BaristaException("sale_state_change_not_found", $"Sale state change with ID '{command.Id}' not found for sale with ID '{command.ParentSaleId}");

            if (command.CausedByUserId != null)
                await _userVerifier.AssertExists(command.CausedByUserId.Value);

            if (command.CausedByPointOfSaleId != null)
                await _posVerifier.AssertExists(command.CausedByPointOfSaleId.Value);

            saleStateChange.SetReason(command.Reason);
            saleStateChange.SetCausedByPointOfSaleId(command.CausedByPointOfSaleId);
            saleStateChange.SetCausedByUserId(command.CausedByUserId);

            await _salesRepository.UpdateAsync(sale);
            await _salesRepository.SaveChanges();

            await _busPublisher.Publish(new SaleStateChangeUpdated(saleStateChange.Id, saleStateChange.Created,
                saleStateChange.Reason, saleStateChange.State.ToString(), saleStateChange.CausedByPointOfSaleId,
                saleStateChange.CausedByUserId));

            return OperationResult.Ok();
        }
    }
}
