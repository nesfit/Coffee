using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.ManualStockOperation;
using Barista.StockOperations.Events.ManualStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;

namespace Barista.StockOperations.Handlers.ManualStockOperation
{
    public class CreateManualStockOperationHandler : ICommandHandler<ICreateManualStockOperation, IIdentifierResult>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IStockItemVerifier _stockItemVerifier;
        private readonly IUserVerifier _userVerifier;

        public CreateManualStockOperationHandler(IStockOperationRepository repository, IBusPublisher busPublisher, IStockItemVerifier stockItemVerifier, IUserVerifier userVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateManualStockOperation command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _stockItemVerifier.AssertExists(command.StockItemId),
                _userVerifier.AssertExists(command.CreatedByUserId)
            );

            var manualStockOp = new Domain.ManualStockOperation(command.Id, command.StockItemId, command.Quantity, command.CreatedByUserId, command.Comment);
            await _repository.AddAsync(manualStockOp);


            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("manual_stock_operation_already_exists", $"A manual stock operation with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new ManualStockOperationCreated(manualStockOp.Id, manualStockOp.StockItemId, manualStockOp.Quantity, manualStockOp.CreatedByUserId, manualStockOp.Comment));
            return new IdentifierResult(manualStockOp.Id);
        }
    }
}
