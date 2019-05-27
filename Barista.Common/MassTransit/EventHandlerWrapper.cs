using System;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Common.MassTransit
{
    public class EventHandlerWrapper<TEvent> : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        private readonly IServiceProvider _serviceProvider;

        public EventHandlerWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                await handler.HandleAsync(context.Message, context.ToCorrelationContext());
            }
        }
    }
}