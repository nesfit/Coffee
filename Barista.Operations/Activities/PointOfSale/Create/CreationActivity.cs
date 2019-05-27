using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSale;
using Barista.Operations.Commands.PointOfSale;
using MassTransit.Courier;

namespace Barista.Operations.Activities.PointOfSale.Create
{
    public class CreationActivity : Activity<CreationActivityParameters, IdentifierLogEntry>
    {
        private readonly IBusPublisher _busPublisher;

        public CreationActivity(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<CreationActivityParameters> context)
        {
            var result = await _busPublisher.SendRequest<ICreatePointOfSale, IIdentifierResult>(
                new CreatePointOfSale(context.Arguments.Id, context.Arguments.DisplayName, context.Arguments.ParentAccountingGroupId, context.Arguments.SaleStrategyId, context.Arguments.Features)
            );

            return result.Successful ? context.Completed(new IdentifierLogEntry(result.Id.Value)) : context.Faulted(result.ToException());
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IdentifierLogEntry> context)
        {
            var result = await _busPublisher.SendRequest<IDeletePointOfSale, IOperationResult>(new DeletePointOfSale(context.Log.Id));
            return result.Successful ? context.Compensated() : context.Failed(result.ToException());
        }
    }
}
