using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Dispatchers;
using Barista.Common.OperationResults;
using Barista.Consistency.Commands;
using Barista.Consistency.Domain;
using Barista.Consistency.Queries;
using Barista.Consistency.Repositories;
using Barista.Contracts;
using ImpromptuInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Barista.Consistency
{
    public class ScheduledEventDispatcherService : IHostedService, IDisposable
    {
        private readonly IServiceScope _scope;

        private readonly ILogger<ScheduledEventDispatcherService> _logger;
        private readonly IScheduledEventRepository _repository;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        private Task _backgroundTask;

        public ScheduledEventDispatcherService(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            
            _scope = serviceProvider.CreateScope();

            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<ScheduledEventDispatcherService>>();
            _repository = _scope.ServiceProvider.GetRequiredService<IScheduledEventRepository>();
            _eventDispatcher = _scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
            _commandDispatcher = _scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        }

        private static readonly TimeSpan Period = TimeSpan.FromSeconds(15);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service is starting.");
            _backgroundTask = Task.Factory.StartNew(() => ProcessScheduledEventsPeriodically(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            _logger.LogInformation("The service started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service is stopping.");
            _backgroundTask?.Wait(cancellationToken);
            _logger.LogInformation("The service stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        private static readonly BrowseDueScheduledEvents Query = new BrowseDueScheduledEvents();

        private async Task ProcessScheduledEventsPeriodically(CancellationToken cancellationToken)
        {
            do
            {
                await ProcessScheduledEvents(cancellationToken);
                await Task.Delay(Period, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        private async Task ProcessScheduledEvents(CancellationToken cancellationToken)
        {
            IEnumerable<ScheduledEvent> dueEvents;

            try
            {
                var pagedResult = await _repository.BrowseAsync(Query);
                dueEvents = pagedResult.Items;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not retrieve scheduled events");
                return;
            }

            var processedEventsCount = 0;

            foreach (var dueEvent in dueEvents)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var eventType = Type.GetType(dueEvent.MessageTypeName);

                if (eventType is null)
                {
                    _logger.LogError($"Could not dispatch scheduled event with fully qualified name {dueEvent.MessageTypeName}, type not found");
                    continue;
                }

                if (!typeof(IEvent).IsAssignableFrom(eventType))
                {
                    _logger.LogError($"Could not dispatch scheduled event with type {eventType} as it does not implement {nameof(IEvent)}");
                    continue;
                }

                IEvent @event = null;

                try
                {
                    var anon = JsonConvert.DeserializeObject(dueEvent.SerializedContents);
                    @event = anon.ActLike(typeof(IEvent), eventType);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Could not deserialize data of event {eventType}: {dueEvent.SerializedContents}");
                }

                try
                {
                    _logger.LogTrace($"Dispatching event {eventType}");
                    await _eventDispatcher.DispatchEvent(@event);
                    processedEventsCount++;

                    await _commandDispatcher.HandleAsync<DeleteScheduledEvent, IOperationResult>(
                        new DeleteScheduledEvent(dueEvent.Id), new CorrelationContext(Guid.Empty, Guid.Empty, Guid.Empty));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Could not dispatch event {eventType}");
                }
            }

            _logger.LogDebug($"Scheduled event dispatcher service dispatched {processedEventsCount} scheduled events.");
        }
    }
}
