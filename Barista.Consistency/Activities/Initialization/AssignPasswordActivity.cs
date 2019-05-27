using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Contracts.Commands.AssignmentToUser;
using MassTransit.Courier;

namespace Barista.Consistency.Activities.Initialization
{
    public class AssignPasswordActivity : ExecuteActivity<UserIdMeansIdParameters>
    {
        private readonly IBusPublisher _busPublisher;

        public AssignPasswordActivity(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<UserIdMeansIdParameters> context)
        {
            var result = await _busPublisher.SendRequest<ICreateAssignmentToUser, IIdentifierResult>(new CreateAssignmentToUser
            {
                Id = Guid.NewGuid(),
                IsShared = false,
                MeansId = context.Arguments.MeansId,
                UserId = context.Arguments.UserId,
                ValidSince = DateTimeOffset.UtcNow
            });

            if (!result.Successful)
                throw result.ToException();

            return context.Completed();
        }
    }
}
