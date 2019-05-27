using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.StockItem;
using Barista.StockItems.Events.StockItem;
using Barista.StockItems.Repositories;
using Barista.StockItems.Verifiers;

namespace Barista.StockItems.Handlers.StockItem
{
    public class CreateStockItemHandler : ICommandHandler<ICreateStockItem, IIdentifierResult>
    {
        private readonly IStockItemRepository _repository;
        private readonly IBusPublisher _busPublisher;
        private readonly IPointOfSaleVerifier _posVerifier;

        public CreateStockItemHandler(IStockItemRepository repository, IBusPublisher busPublisher, IPointOfSaleVerifier posVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateStockItem command, ICorrelationContext context)
        {
            await _posVerifier.AssertExists(command.PointOfSaleId);

            var stockItem = new Domain.StockItem(command.Id, command.DisplayName, command.PointOfSaleId);
            await _repository.AddAsync(stockItem);
            
            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("stock_item_already_exists", $"A stock item with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new StockItemCreated(stockItem.Id, stockItem.DisplayName, stockItem.PointOfSaleId));
            return new IdentifierResult(stockItem.Id);
        }
    }
}
