using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SaleBasedStockOperation;
using Barista.StockOperations.Events.SaleBasedStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;

namespace Barista.StockOperations.Handlers.SaleBasedStockOperation
{
    public class UpdateSaleBasedStockOperationHandler : ICommandHandler<IUpdateSaleBasedStockOperation, IOperationResult>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IStockItemVerifier _stockItemVerifier;
        private readonly ISaleVerifier _saleVerifier;

        public UpdateSaleBasedStockOperationHandler(IStockOperationRepository repository, IBusPublisher busPublisher, IStockItemVerifier stockItemVerifier, ISaleVerifier saleVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
            _saleVerifier = saleVerifier ?? throw new ArgumentNullException(nameof(saleVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateSaleBasedStockOperation command, ICorrelationContext context)
        {
            var stockOp = await _repository.GetAsync(command.Id);
            if (stockOp is null)
                throw new BaristaException("stock_operation_not_found", $"Could not find stock operation with ID '{command.Id}'");
            if (!(stockOp is Domain.SaleBasedStockOperation saleBasedStockOp))
                throw new BaristaException("invalid_stock_operation_update_command", $"The stock operation with ID '{command.Id}' is not a sale-based stock operation and therefore cannot be updated with this command.");

            await Task.WhenAll(
                _stockItemVerifier.AssertExists(command.StockItemId),
                _saleVerifier.AssertExists(command.SaleId)
            );

            saleBasedStockOp.SetQuantity(command.Quantity);
            saleBasedStockOp.SetStockItem(command.StockItemId);
            saleBasedStockOp.SetSaleId(command.SaleId);

            await _repository.UpdateAsync(saleBasedStockOp);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new SaleBasedStockOperationUpdated(saleBasedStockOp.Id, saleBasedStockOp.StockItemId, saleBasedStockOp.Quantity, saleBasedStockOp.SaleId));
            return OperationResult.Ok();
        }
    }
}
