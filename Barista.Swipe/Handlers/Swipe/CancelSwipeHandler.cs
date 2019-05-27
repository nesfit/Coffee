using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Swipe;
using Barista.Swipe.Commands;
using Barista.Swipe.Services;

namespace Barista.Swipe.Handlers.Swipe
{
    public class CancelSwipeHandler : ICommandHandler<ICancelSwipe, IOperationResult>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IAccountingService _accountingService;

        public CancelSwipeHandler(IBusPublisher busPublisher, IAccountingService accountingService)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
        }

        public async Task<IOperationResult> HandleAsync(ICancelSwipe command, ICorrelationContext correlationContext)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var sale = await _accountingService.GetSale(command.SaleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Could not find sale with ID '{command.SaleId}'");
            if (sale.PointOfSaleId != command.PointOfSaleId)
                throw new BaristaException("unauthorized_point_of_sale", $"The sale with ID '{command.SaleId}' was not created by the requesting PoS with ID '{command.PointOfSaleId}'");

            var opResult = await _busPublisher.SendRequest<ChangeSaleStateToCancelled, IOperationResult>(
                new ChangeSaleStateToCancelled(command.SaleId, Guid.NewGuid(), "PoS", command.PointOfSaleId)
            );

            if (opResult.Successful)
                return OperationResult.Ok();
            else
                throw opResult.ToException();
        }
    }
}
