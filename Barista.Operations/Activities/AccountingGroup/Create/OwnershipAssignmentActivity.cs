using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Operations.Commands.AccountingGroupUserAuthorization;
using MassTransit.Courier;

namespace Barista.Operations.Activities.AccountingGroup.Create
{
    public class OwnershipAssignmentActivity : ExecuteActivity<OwnershipAssignmentParameters>
    {
        private readonly IBusPublisher _busPublisher;

        public OwnershipAssignmentActivity(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<OwnershipAssignmentParameters> context)
        {
            var result = await _busPublisher.SendRequest<CreateAccountingGroupOwnership, IParentChildIdentifierResult>(
                new CreateAccountingGroupOwnership(context.Arguments.AccountingGroupId, context.Arguments.OwnerUserId)
            );

            return result.Successful ? context.Completed() : context.Faulted(new Exception(result.ErrorCode));
        }
    }
}
