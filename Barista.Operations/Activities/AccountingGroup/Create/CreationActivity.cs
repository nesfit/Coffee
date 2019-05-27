using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Operations.Commands.AccountingGroup;
using MassTransit.Courier;

namespace Barista.Operations.Activities.AccountingGroup.Create
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
            var result = await _busPublisher.SendRequest<CreateAccountingGroup, IIdentifierResult>(
                new CreateAccountingGroup(context.Arguments.Id, context.Arguments.DisplayName, context.Arguments.SaleStrategyId)
            );
            
            if (!result.Successful)
                return context.Faulted(result.ToException());

            return context.Completed(new IdentifierLogEntry(result.Id.Value));
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IdentifierLogEntry> context)
        {
            var result = await _busPublisher.SendRequest<DeleteAccountingGroup, IOperationResult>(new DeleteAccountingGroup(context.Log.Id));
            return result.Successful ? context.Compensated() : context.Failed(result.ToException());
        }
    }
}
