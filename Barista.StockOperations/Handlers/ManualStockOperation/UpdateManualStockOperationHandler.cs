using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.ManualStockOperation;
using Barista.StockOperations.Events.ManualStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;

namespace Barista.StockOperations.Handlers.ManualStockOperation
{
    public class UpdateManualStockOperationHandler : ICommandHandler<IUpdateManualStockOperation, IOperationResult>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IStockItemVerifier _stockItemVerifier;
        private readonly IUserVerifier _userVerifier;

        public UpdateManualStockOperationHandler(IStockOperationRepository repository, IBusPublisher busPublisher, IStockItemVerifier stockItemVerifier, IUserVerifier userVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateManualStockOperation command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var stockOp = await _repository.GetAsync(command.Id);
            if (stockOp is null)
                throw new BaristaException("stock_operation_not_found", $"Could not find stock operation with ID '{command.Id}'");

            if (!(stockOp is Domain.ManualStockOperation manualStockOp))
                throw new BaristaException("invalid_stock_operation_update_command", $"The stock operation with ID '{command.Id}' is not a manual stock operation and therefore cannot be updated with this command.");

            await Task.WhenAll(
                _stockItemVerifier.AssertExists(command.StockItemId),
                _userVerifier.AssertExists(command.CreatedByUserId)
            );

            manualStockOp.SetQuantity(command.Quantity);
            manualStockOp.SetStockItem(command.StockItemId);
            manualStockOp.SetComment(command.Comment);
            manualStockOp.SetCreatedByUserId(command.CreatedByUserId);

            await _repository.UpdateAsync(manualStockOp);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new ManualStockOperationUpdated(manualStockOp.Id, manualStockOp.StockItemId, manualStockOp.Quantity, manualStockOp.CreatedByUserId, manualStockOp.Comment));
            return OperationResult.Ok();
        }
    }
}
