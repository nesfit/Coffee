using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Queries;
using Barista.Consistency.Services;
using Barista.Contracts.Events.SaleStateChange;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Handlers.SaleStateChange
{
    public class SaleStateChangeCreatedHandler : IEventHandler<ISaleStateChangeCreated>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IAccountingService _accountingService;
        private readonly IOffersService _offersService;
        private readonly IStockOperationsService _stockOperationsService;
        private readonly ILogger _logger;

        public SaleStateChangeCreatedHandler(IBusPublisher busPublisher, IAccountingService accountingService, IOffersService offersService, IStockOperationsService stockOperationsService, ILogger<SaleStateChangeCreatedHandler> logger)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
            _offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
            _stockOperationsService = stockOperationsService ?? throw new ArgumentNullException(nameof(stockOperationsService));
            _logger = logger;
        }

        public async Task HandleAsync(ISaleStateChangeCreated @event, ICorrelationContext correlationContext)
        {
            switch (@event.State)
            {
                case "Cancelled":
                    await EnsureRemovalOfSaleBasedOp(@event.SaleId);
                    break;

                case "Confirmed":
                    await EnsureCreationOfSaleBasedOp(@event.SaleId);
                    break;
            }
        }

        protected async Task EnsureRemovalOfSaleBasedOp(Guid saleId)
        {
            var sale = await _accountingService.GetSale(saleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Error retrieving sale with ID {saleId} that supposedly changed state.");

            var offer = await _offersService.GetOffer(sale.OfferId);
            if (offer is null)
            {
                _logger.LogInformation($"The sale {saleId} changed state to cancelled but its offer could not be found");
                return;
            }

            if (!(offer.StockItemId is Guid stockItemId))
                return;

            var autoStockOps = await _stockOperationsService.BrowseSaleBasedStockOperations(new BrowseSaleBasedStockOperations { StockItemId = new Guid[] { stockItemId }, SaleId = sale.Id });
            if (autoStockOps.TotalResults == 0)
                return;
            else if (autoStockOps.TotalResults > 1)
                throw new BaristaException("multiple_stock_ops_per_one_sale", $"Multiple stock operations yielded by query of stockItemId={stockItemId} and saleId={sale.Id}");

            var autoStockOp = autoStockOps.Items.Single();
            var removalOpResult = await _busPublisher.SendRequest(new DeleteSaleBasedStockOperation(autoStockOp.Id));
            if (!removalOpResult.Successful)
                throw removalOpResult.ToException();
        }

        protected async Task EnsureCreationOfSaleBasedOp(Guid saleId)
        {
            var sale = await _accountingService.GetSale(saleId);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Error retrieving sale with ID {saleId} that supposedly changed state.");

            var offer = await _offersService.GetOffer(sale.OfferId);
            if (offer is null)
            {
                _logger.LogInformation($"The sale {saleId} changed state to cancelled but its offer could not be found");
                return;
            }

            if (!(offer.StockItemId is Guid stockItemId))
                return;

            var autoStockOps = await _stockOperationsService.BrowseSaleBasedStockOperations(new BrowseSaleBasedStockOperations { StockItemId = new Guid[] { stockItemId }, SaleId = sale.Id });
            if (autoStockOps.TotalResults > 0)
                return;

            var creationOpResult = await _busPublisher.SendRequest(new CreateSaleBasedStockOperation(Guid.NewGuid(), stockItemId, -sale.Quantity, sale.Id));
            if (!creationOpResult.Successful)
                throw creationOpResult.ToException();
        }
    }
}
