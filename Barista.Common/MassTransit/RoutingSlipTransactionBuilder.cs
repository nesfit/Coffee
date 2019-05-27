using System;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;

namespace Barista.Common.MassTransit
{
    public class RoutingSlipTransactionBuilder : IRoutingSlipTransactionBuilder
    {
        private readonly IBusControl _busControl;
        private readonly RoutingSlipBuilder _routingSlip;

        public RoutingSlipTransactionBuilder(IBusControl busControl)
        {
            _busControl = busControl;
            _routingSlip = new RoutingSlipBuilder(NewId.NextGuid());
        }

        public IRoutingSlipTransactionBuilder Add<TActivity>()
        {
            _routingSlip.AddActivity(typeof(TActivity).FullName, _busControl.Address.AppendActivityExchangeName(typeof(TActivity)));
            return this;
        }

        public IRoutingSlipTransactionBuilder Add<TActivity, TActivityParams>(TActivityParams @params)
        {
            _routingSlip.AddActivity(typeof(TActivity).FullName, _busControl.Address.AppendActivityExchangeName(typeof(TActivity)), @params);
            return this;
        }

        public IRoutingSlipTransactionBuilder SetVariables(object variables)
        {
            _routingSlip.SetVariables(variables);
            return this;
        }

        public async Task<Guid> StartAsync()
        {
            var queueUri = new Uri(_busControl.Address, Assembly.GetEntryAssembly().GenerateRoutingSlipFailureQueueName());
            _routingSlip.AddSubscription(queueUri,
                RoutingSlipEvents.Faulted | RoutingSlipEvents.ActivityCompensationFailed |
                RoutingSlipEvents.ActivityFaulted | RoutingSlipEvents.CompensationFailed);
            
            await _busControl.Execute(_routingSlip.Build());
            return _routingSlip.TrackingNumber;
        }
    }
}
