using System;
using System.Threading.Tasks;
using Barista.Accounting.Commands;
using Barista.Accounting.Domain;
using Barista.Accounting.Queries;
using Barista.Common;
using Barista.Common.Dispatchers;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Sale;
using Barista.Contracts.Commands.SaleStateChange;

namespace Barista.Accounting.Handlers.Sale
{
    public class CancelTimedOutSaleHandler : ICommandHandler<ICancelTimedOutSale, IOperationResult>
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IBusPublisher _busPublisher;

        public CancelTimedOutSaleHandler(IQueryDispatcher queryDispatcher, IBusPublisher busPublisher)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(ICancelTimedOutSale command, ICorrelationContext correlationContext)
        {
            var sale = await _queryDispatcher.QueryAsync(new GetSale(command.SaleId));
            if (sale is null)
                return new OperationResult("sale_not_found", $"Could not find sale with ID '{command.SaleId}'");

            if (sale.State != SaleState.FundsReserved.ToString())
                return new OperationResult("sale_not_in_appropriate_state", $"The sale with ID '{command.SaleId}' is in the state '{sale.State}', cancellation can only happen in the {nameof(SaleState.FundsReserved)}");

            var newSaleStateChange = await _busPublisher.SendRequest<ICreateSaleStateChange, IIdentifierResult>(
                new CreateSaleStateChange(command.SaleId, Guid.NewGuid(), "Timeout", "Cancelled", null, null)
            );

            return newSaleStateChange;
        }
    }
}
