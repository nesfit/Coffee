using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.User;
using Barista.Contracts.Events.User;

namespace Barista.Consistency.Handlers.User
{
    public class UserDeletedHandler : IEventHandler<IUserDeleted>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public UserDeletedHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }


        public async Task HandleAsync(IUserDeleted @event, ICorrelationContext correlationContext)
            => await _transactionBuilderFactory.Create()
                .Add<DeleteOrphanedAccountingGroupAuthorizationsActivity>()
                .Add<DeleteOrphanedPointOfSaleAuthorizationsActivity>()
                .Add<DeleteOrphanedMeansAssignmentsActivity>().SetVariables(new UserIdParameters { UserId = @event.Id, SourceEvent = @event })
                .Add<RepeatIfRequiredActivity>()
                .StartAsync();
    }
}
