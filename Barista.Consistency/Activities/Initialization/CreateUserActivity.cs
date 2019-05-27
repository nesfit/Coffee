using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Domain;
using Barista.Consistency.Repositories;
using Barista.Contracts.Commands.User;
using Barista.Contracts.Events.Consistency;
using MassTransit.Courier;

namespace Barista.Consistency.Activities.Initialization
{
    public class CreateUserActivity : Activity<EmptyParameters, GuidLog>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly IScheduledEventRepository _scheduledEventRepo;

        public CreateUserActivity(IBusPublisher busPublisher, IScheduledEventRepository scheduledEventRepo)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _scheduledEventRepo = scheduledEventRepo ?? throw new ArgumentNullException(nameof(scheduledEventRepo));
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<EmptyParameters> context)
        {
            var result = await _busPublisher.SendRequest<ICreateUser, IIdentifierResult>(new CreateUser
            {
                EmailAddress = "admin@example.com",
                FullName = "Default Administrator Account",
                Id = Guid.NewGuid(),
                IsAdministrator = true,
                IsActive = true
            });

            if (!result.Successful)
                throw result.ToException();

            var userId = result.Id.Value;
            return context.CompletedWithVariables(new GuidLog {Id = userId}, new UserIdParameters() {UserId = userId});
        }

        public async Task<CompensationResult> Compensate(CompensateContext<GuidLog> context)
        {
            var result = await _busPublisher.SendRequest(new DeleteUser {Id = context.Log.Id});
            if (!result.Successful)
                throw result.ToException();

            await _scheduledEventRepo.AddAsync(new ScheduledEvent(Guid.NewGuid(), typeof(IDatabaseCreated).AssemblyQualifiedName, "{}", DateTimeOffset.UtcNow));
            await _scheduledEventRepo.SaveChanges();

            return context.Compensated();
        }
    }
}
