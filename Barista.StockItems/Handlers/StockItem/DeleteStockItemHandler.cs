using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.StockItem;
using Barista.StockItems.Events.StockItem;
using Barista.StockItems.Repositories;

namespace Barista.StockItems.Handlers.StockItem
{
    public class DeleteStockItemHandler : ICommandHandler<IDeleteStockItem, IOperationResult>
    {
        private readonly IStockItemRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteStockItemHandler(IStockItemRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteStockItem command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var stockItem = await _repository.GetAsync(command.Id);
            if (stockItem is null)
                throw new BaristaException("stock_item_not_found", $"Could not find stock item with ID '{command.Id}'");

            await _repository.DeleteAsync(stockItem);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new StockItemDeleted(stockItem.Id));
            return OperationResult.Ok();
        }
    }
}
