using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Operations;
using Barista.Operations.Activities.AccountingGroup.Create;

namespace Barista.Operations.Handlers.Operations
{
    public class HandleCreationOfAccountingGroupHandler : ICommandHandler<IHandleCreationOfAccountingGroup, ILongRunningOperationResult>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public HandleCreationOfAccountingGroupHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task<ILongRunningOperationResult> HandleAsync(IHandleCreationOfAccountingGroup command, ICorrelationContext correlationContext)
        {
            var transactionBuilder = _transactionBuilderFactory.Create()
                .Add<CreationActivity>()
                .Add<OwnershipAssignmentActivity>()
                .SetVariables(command);

            var operationId = await transactionBuilder.StartAsync();
            return new LongRunningOperationResult(operationId);
        }
    }
}
