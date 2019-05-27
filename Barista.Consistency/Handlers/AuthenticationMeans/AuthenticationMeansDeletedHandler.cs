using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.AuthenticationMeans;
using Barista.Contracts.Events.AuthenticationMeans;

namespace Barista.Consistency.Handlers.AuthenticationMeans
{
    public class AuthenticationMeansDeletedHandler : IEventHandler<IAuthenticationMeansDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public AuthenticationMeansDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }


        public async Task HandleAsync(IAuthenticationMeansDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                    .Add<DeleteOrphanedAssignmentsToPointOfSaleActivity>()
                    .Add<DeleteOrphanedAssignmentsToUserActivity>()
                    .Add<RepeatIfRequiredActivity>()
                    .SetVariables(new AuthenticationMeansIdParameters {AuthenticationMeansId = @event.Id, SourceEvent = @event})
                    .StartAsync();
    }
}
