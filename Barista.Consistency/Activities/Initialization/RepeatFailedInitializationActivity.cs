using System;
using System.Threading.Tasks;
using Barista.Consistency.Domain;
using Barista.Consistency.Repositories;
using Barista.Contracts.Events.Consistency;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities.Initialization
{
    public class RepeatFailedInitializationActivity : Activity<EmptyParameters, GuidLog>
    {
        private readonly IScheduledEventRepository _repository;
        private readonly ILogger _logger;

        public RepeatFailedInitializationActivity(IScheduledEventRepository repository, ILogger<RepeatFailedInitializationActivity> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<ExecutionResult> Execute(ExecuteContext<EmptyParameters> context)
        {
            return Task.FromResult(context.Completed(new GuidLog()));
        }

        public async Task<CompensationResult> Compensate(CompensateContext<GuidLog> context)
        {
            _logger.LogWarning("Database initialization failed, re-scheduling original event in 30 seconds.");
            await _repository.AddAsync(new ScheduledEvent(Guid.NewGuid(), typeof(IDatabaseCreated).AssemblyQualifiedName, "{}", DateTimeOffset.UtcNow.AddSeconds(30)));
            await _repository.SaveChanges();
            _logger.LogInformation("Another attempt to initialize the database will happen in 30 seconds.");
            return context.Compensated();
        }
    }
}
