using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public EventDispatcher(IServiceProvider serviceProvider, ILogger<EventDispatcher> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static Task InvokeHandler(IEvent @event, Type handledEventType, object eventHandler)
        {
            return (Task) typeof(EventDispatcher)
                .GetMethod(nameof(InvokeHandlerGeneric), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(handledEventType)
                .Invoke(null, new [] { @event, eventHandler });
        }

        private static async Task InvokeHandlerGeneric<TEvent>(TEvent @event, IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            await eventHandler.HandleAsync(@event, new CorrelationContext(Guid.Empty, Guid.Empty, Guid.Empty));
        }

        public async Task DispatchEvent(IEvent @event)
        {
            var eventInterface = @event.GetType().GetInterfaces().Single(e => e.GetInterfaces().Contains(typeof(IEvent)));
            var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventInterface);
            var eventHandler = _serviceProvider.GetRequiredService(eventHandlerType);

            await InvokeHandler(@event, eventInterface, eventHandler);
        }
    }
}
