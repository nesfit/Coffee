using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.StockItem;
using Barista.Contracts.Events.StockItem;

namespace Barista.Consistency.Handlers.StockItem
{
    public class StockItemDeletedHandler : IEventHandler<IStockItemDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public StockItemDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task HandleAsync(IStockItemDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                    .Add<UnsetOfferReferenceActivity>()
                    .Add<DeleteOrphanedManualStockOperationsActivity>()
                    .Add<DeleteOrphanedSaleBasedStockOperationsActivity>()
                    .Add<RepeatIfRequiredActivity>()
                    .SetVariables(new StockItemIdParameters { StockItemId = @event.Id, SourceEvent = @event })
                    .StartAsync();
    }
}
