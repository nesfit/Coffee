using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.Sale;
using Barista.Contracts.Events.Sale;

namespace Barista.Consistency.Handlers.Sale
{
    public class SaleDeletedHandler : IEventHandler<ISaleDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public SaleDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task HandleAsync(ISaleDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                    .Add<DeleteOrphanedSaleBasedStockOperationsActivity>()
                    .Add<RepeatIfRequiredActivity>()
                    .SetVariables(new SaleIdParameters { SaleId = @event.Id, SourceEvent = @event })
                    .StartAsync();
    }
}
