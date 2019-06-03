using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Consistency.Activities.Initialization;
using Barista.Contracts.Events.Consistency;

namespace Barista.Consistency.Handlers
{
    public class DatabaseCreatedEventHandler : IEventHandler<IDatabaseCreated>
    {
        private readonly IRoutingSlipTransactionBuilderFactory _factory;

        public DatabaseCreatedEventHandler(IRoutingSlipTransactionBuilderFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task HandleAsync(IDatabaseCreated @event, ICorrelationContext correlationContext)
        {
            await _factory.Create()
                .Add<RepeatFailedInitializationActivity>()
                .Add<CreateUserActivity>()
                .Add<CreatePasswordActivity>()
                .Add<AssignPasswordActivity>()
                .StartAsync();
        }
    }
}
