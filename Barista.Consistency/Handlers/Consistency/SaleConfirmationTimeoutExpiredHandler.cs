using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Events;
using Barista.Consistency.Services;
using Barista.Contracts.Events.Sale;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Handlers.Consistency
{
    public class SaleConfirmationTimeoutExpiredHandler : IEventHandler<ISaleConfirmationTimeoutExpired>
    {
        private readonly IAccountingService _accSvc;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger _logger;

        public SaleConfirmationTimeoutExpiredHandler(IAccountingService accSvc, IBusPublisher busPublisher, ILogger<SaleConfirmationTimeoutExpiredHandler> logger)
        {
            _accSvc = accSvc ?? throw new ArgumentNullException(nameof(accSvc));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(ISaleConfirmationTimeoutExpired @event, ICorrelationContext correlationContext)
        {
            var sale = await _accSvc.GetSale(@event.SaleId);
            if (sale is null)
            {
                _logger.LogDebug($"The sale {@event.SaleId} does not exist.");
                return;
            }
            else if (sale.State == "Confirmed" || sale.State == "Cancelled")
            {
                _logger.LogDebug($"The sale {@event.SaleId} is already in a permitted state of {sale.State}.");
                return;
            }

            _logger.LogInformation($"The sale {@event.SaleId} is in a non-final state of {sale.State}, requesting its cancellation.");

            var deletionResult = await _busPublisher.SendRequest(new CancelTimedOutSale(@event.SaleId));

            if (deletionResult.Successful)
                _logger.LogDebug($"The sale {@event.SaleId} was cancelled successfully due to timing out.");
            else
                _logger.LogInformation(deletionResult.ToException(), $"The sale {@event.SaleId} was not cancelled due to timing out.");
        }
    }
}
