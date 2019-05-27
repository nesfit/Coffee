using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities;
using Barista.Consistency.Activities.AccountingGroup;
using Barista.Contracts.Events.AccountingGroup;

namespace Barista.Consistency.Handlers.AccountingGroup
{
    public class AccountingGroupDeletedHandler : IEventHandler<IAccountingGroupDeleted>
    {
        private readonly IRoutingSlipTransactionBuilder _transactionBuilder;

        public AccountingGroupDeletedHandler(IRoutingSlipTransactionBuilder transactionBuilder)
        {
            _transactionBuilder = transactionBuilder ?? throw new ArgumentNullException(nameof(transactionBuilder));
        }

        public async Task HandleAsync(IAccountingGroupDeleted @event, ICorrelationContext correlationContext)
        {
            _transactionBuilder.Add<DeleteOrphanedAuthorizationsActivity>();
            _transactionBuilder.Add<RepeatIfRequiredActivity>();
            _transactionBuilder.SetVariables(new AccountingGroupIdParameters { AccountingGroupId = @event.Id, SourceEvent = @event});
            await _transactionBuilder.StartAsync();
        }
    }
}
