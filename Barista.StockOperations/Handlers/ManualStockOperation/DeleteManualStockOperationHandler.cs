using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.ManualStockOperation;
using Barista.StockOperations.Events.ManualStockOperation;
using Barista.StockOperations.Repositories;

namespace Barista.StockOperations.Handlers.ManualStockOperation
{
    public class DeleteManualStockOperationHandler : ICommandHandler<IDeleteManualStockOperation, IOperationResult>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteManualStockOperationHandler(IStockOperationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteManualStockOperation command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var stockOp = await _repository.GetAsync(command.Id);
            if (stockOp is null)
                throw new BaristaException("stock_operation_not_found", $"Could not find stock operation with ID '{command.Id}'");
            if (!(stockOp is Domain.ManualStockOperation))
                throw new BaristaException("invalid_stock_operation_delete_command", $"The stock operation with ID '{command.Id}' is not a manual stock operation and therefore cannot be deleted with this command.");

            await _repository.DeleteAsync(stockOp);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new ManualStockOperationDeleted(stockOp.Id));
            return OperationResult.Ok();
        }
    }
}
