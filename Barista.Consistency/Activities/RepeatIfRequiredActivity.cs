using System;
using System.Threading.Tasks;
using Barista.Consistency.Domain;
using Barista.Consistency.Repositories;
using MassTransit.Courier;

namespace Barista.Consistency.Activities
{
    public class RepeatIfRequiredActivity : ExecuteActivity<ConsistencyActivityParametersBase>
    {
        private readonly IConsistencyConfiguration _consistencyConfiguration;
        private readonly IScheduledEventRepository _scheduledEventRepository;

        public RepeatIfRequiredActivity(IConsistencyConfiguration consistencyConfiguration, IScheduledEventRepository scheduledEventRepository)
        {
            _consistencyConfiguration = consistencyConfiguration;
            _scheduledEventRepository = scheduledEventRepository;
        }

        protected async Task ScheduleRepeatedRunAsync(ISourceEventData sourceEventData, bool shouldBeDelayed)
        {
            var delay = shouldBeDelayed
                ? _consistencyConfiguration.DelayedRepeatedRunInterval
                : _consistencyConfiguration.ImmediateRepeatedRunInterval;

            var scheduledRun = new ScheduledEvent(
                Guid.NewGuid(),
                sourceEventData.FullyQualifiedEventTypeName,
                sourceEventData.EventData,
                DateTimeOffset.UtcNow.Add(delay)
            );

            await _scheduledEventRepository.AddAsync(scheduledRun);
            await _scheduledEventRepository.SaveChanges();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ConsistencyActivityParametersBase> context)
        {
            if (context.Arguments.RerunRequiredTimes > 0)
                await ScheduleRepeatedRunAsync(context.Arguments.SourceEventData, false);
            else if (DateTimeOffset.UtcNow.Subtract(context.Arguments.SourceEventData.CreatedAt) < _consistencyConfiguration.ConsistencyTaskLifetime)
                await ScheduleRepeatedRunAsync(context.Arguments.SourceEventData, true);

            return context.Completed();
        }
    }
}
