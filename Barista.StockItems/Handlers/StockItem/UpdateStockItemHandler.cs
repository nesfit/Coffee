using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.StockItem;
using Barista.StockItems.Events.StockItem;
using Barista.StockItems.Repositories;
using Barista.StockItems.Verifiers;

namespace Barista.StockItems.Handlers.StockItem
{
    public class UpdateStockItemHandler : ICommandHandler<IUpdateStockItem, IOperationResult>
    {
        private readonly IStockItemRepository _repository;
        private readonly IBusPublisher _busPublisher;
        private readonly IPointOfSaleVerifier _posVerifier;

        public UpdateStockItemHandler(IStockItemRepository repository, IBusPublisher busPublisher, IPointOfSaleVerifier posVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateStockItem command, ICorrelationContext context)
        {
            var stockItem = await _repository.GetAsync(command.Id);
            if (stockItem is null)
                throw new BaristaException("stock_item_not_found", $"Could not find stock item with ID '{command.Id}'");

            await _posVerifier.AssertExists(command.PointOfSaleId);

            stockItem.SetDisplayName(command.DisplayName);
            stockItem.SetPointOfSaleId(command.PointOfSaleId);
            await _repository.UpdateAsync(stockItem);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new StockItemUpdated(stockItem.Id, stockItem.DisplayName, stockItem.PointOfSaleId));
            return OperationResult.Ok();
        }
    }
}
