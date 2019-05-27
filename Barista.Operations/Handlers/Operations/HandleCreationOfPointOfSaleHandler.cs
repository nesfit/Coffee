using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Operations;
using Barista.Operations.Activities.PointOfSale.Create;

namespace Barista.Operations.Handlers.Operations
{
    public class HandleCreationOfPointOfSaleHandler : ICommandHandler<IHandleCreationOfPointOfSale, ILongRunningOperationResult>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _transactionBuilderFactory;

        public HandleCreationOfPointOfSaleHandler(IRoutingSlipTransactionBuilderFactory transactionBuilderFactory)
        {
            _transactionBuilderFactory = transactionBuilderFactory ?? throw new ArgumentNullException(nameof(transactionBuilderFactory));
        }

        public async Task<ILongRunningOperationResult> HandleAsync(IHandleCreationOfPointOfSale command, ICorrelationContext correlationContext)
        {
            var transactionBuilder = _transactionBuilderFactory.Create()
                .Add<CreationActivity>()
                .Add<OwnershipAssignmentActivity>()
                .SetVariables(command);

            return new LongRunningOperationResult(await transactionBuilder.StartAsync());
        }
    }
}
