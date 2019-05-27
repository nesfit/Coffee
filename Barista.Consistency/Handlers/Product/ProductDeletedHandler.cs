using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.Product;
using Barista.Contracts.Events.Product;

namespace Barista.Consistency.Handlers.Product
{
    public class ProductDeletedHandler : IEventHandler<IProductDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public ProductDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task HandleAsync(IProductDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                .Add<DeleteOrphanedOffersActivity>()
                .Add<RepeatIfRequiredActivity>()
                .SetVariables(new ProductIdParameters() { ProductId = @event.Id, SourceEvent = @event })
                .StartAsync();
    }
}
