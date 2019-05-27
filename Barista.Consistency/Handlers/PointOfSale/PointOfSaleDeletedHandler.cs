using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.PointOfSale;
using Barista.Contracts.Events.PointOfSale;

namespace Barista.Consistency.Handlers.PointOfSale
{
    public class PointOfSaleDeletedHandler : IEventHandler<IPointOfSaleDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public PointOfSaleDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task HandleAsync(IPointOfSaleDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                .Add<DeleteOrphanedAssignmentsActivity>()
                .Add<DeleteOrphanedAuthorizationsActivity>()
                .Add<DeleteOrphanedOffersActivity>()
                .Add<RepeatIfRequiredActivity>()
                .SetVariables(new PointOfSaleIdParameters { PointOfSaleId = @event.Id, SourceEvent = @event })
                .StartAsync();
    }
}
