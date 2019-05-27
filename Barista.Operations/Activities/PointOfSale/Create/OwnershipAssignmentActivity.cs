using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;
using Barista.Operations.Commands.PointOfSaleUserAuthorization;
using MassTransit.Courier;

namespace Barista.Operations.Activities.PointOfSale.Create
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
            var result = await _busPublisher.SendRequest<ICreatePointOfSaleUserAuthorization, IParentChildIdentifierResult>(
                new CreatePointOfSaleOwnership(context.Arguments.PointOfSaleId, context.Arguments.OwnerUserId)
            );

            return result.Successful ? context.Completed() : context.Faulted(result.ToException());
        }
    }
}
