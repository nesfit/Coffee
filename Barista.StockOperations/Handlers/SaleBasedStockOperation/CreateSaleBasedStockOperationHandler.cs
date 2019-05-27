using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SaleBasedStockOperation;
using Barista.StockOperations.Events.SaleBasedStockOperation;
using Barista.StockOperations.Repositories;
using Barista.StockOperations.Verifiers;

namespace Barista.StockOperations.Handlers.SaleBasedStockOperation
{
    public class CreateSaleBasedStockOperationHandler : ICommandHandler<ICreateSaleBasedStockOperation, IIdentifierResult>
    {
        private readonly IStockOperationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IStockItemVerifier _stockItemVerifier;
        private readonly ISaleVerifier _saleVerifier;

        public CreateSaleBasedStockOperationHandler(IStockOperationRepository repository, IBusPublisher busPublisher, IStockItemVerifier stockItemVerifier, ISaleVerifier saleVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _stockItemVerifier = stockItemVerifier ?? throw new ArgumentNullException(nameof(stockItemVerifier));
            _saleVerifier = saleVerifier ?? throw new ArgumentNullException(nameof(saleVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateSaleBasedStockOperation command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _stockItemVerifier.AssertExists(command.StockItemId),
                _saleVerifier.AssertExists(command.SaleId)
            );

            var saleBasedStockOp = new Domain.SaleBasedStockOperation(command.Id, command.StockItemId, command.Quantity, command.SaleId);
            await _repository.AddAsync(saleBasedStockOp);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("sale_based_stock_operation_already_exists", $"A sale-based stock operation with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new SaleBasedStockOperationCreated(saleBasedStockOp.Id, saleBasedStockOp.StockItemId, saleBasedStockOp.Quantity, saleBasedStockOp.SaleId));
            return new IdentifierResult(saleBasedStockOp.Id);
        }
    }
}
