using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Domain;
using Barista.Consistency.Events;
using Barista.Consistency.Repositories;
using Barista.Contracts.Events.Sale;
using Newtonsoft.Json;

namespace Barista.Consistency.Handlers.Sale
{
    public class SaleCreatedHandler : IEventHandler<ISaleCreated>
    {
        private readonly IScheduledEventRepository _scheduledEventRepository;
        private readonly IConsistencyConfiguration _consistencyConfiguration;

        public SaleCreatedHandler(IScheduledEventRepository scheduledEventRepository, IConsistencyConfiguration consistencyConfiguration)
        {
            _scheduledEventRepository = scheduledEventRepository ?? throw new ArgumentNullException(nameof(scheduledEventRepository));
            _consistencyConfiguration = consistencyConfiguration ?? throw new ArgumentNullException(nameof(consistencyConfiguration));
        }

        public async Task HandleAsync(ISaleCreated @event, ICorrelationContext correlationContext)
        {
            var @expirationEvent = new SaleConfirmationTimeoutExpired {SaleId = @event.Id};

            var scheduledRun = new ScheduledEvent(
                Guid.NewGuid(),
                typeof(ISaleConfirmationTimeoutExpired).AssemblyQualifiedName,
                JsonConvert.SerializeObject(expirationEvent),
                DateTimeOffset.UtcNow.Add(_consistencyConfiguration.UnconfirmedSaleExpirationInterval)
            );

            await _scheduledEventRepository.AddAsync(scheduledRun);
            await _scheduledEventRepository.SaveChanges();
        }
    }
}
