using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Mqtt.MessageHandlers;
using Barista.Mqtt.Services;
using Barista.Mqtt.TopicClassifiers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json.Linq;

namespace Barista.Mqtt
{
    public class MqttListenerService : IHostedService
    {
        private readonly ILogger<MqttListenerService> _logger;
        private readonly IPointsOfSaleService _posService;
        private readonly IPosTopicClassifier _posTopicClassifier;
        private readonly IManagedMqttClient _client;
        private readonly IManagedMqttClientOptions _options;
        private readonly IMqttMessageHandler[] _messageHandlers;

        private Task _backgroundTask;
        private CancellationToken _cancellationToken;

        public MqttListenerService(IManagedMqttClient client, IManagedMqttClientOptions options, IConfiguration configuration, IServiceId serviceId,
            ILogger<MqttListenerService> logger, IPointsOfSaleService posService, IServiceProvider serviceProvider, IPosTopicClassifier posTopicClassifier)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (serviceId == null) throw new ArgumentNullException(nameof(serviceId));

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _posService = posService ?? throw new ArgumentNullException(nameof(posService));
            _posTopicClassifier = posTopicClassifier ?? throw new ArgumentNullException(nameof(posTopicClassifier));

            _client.UseApplicationMessageReceivedHandler(OnMqttMessageReceived);
            _client.UseConnectedHandler(args => OnConnectedHandler(args));
            _client.UseDisconnectedHandler(args => OnDisconnectedHandler(args));

            _messageHandlers = serviceProvider.GetServices<IMqttMessageHandler>().ToArray();
        }

        protected async Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            _logger.LogTrace("MQTT message received on {topic}", args.ApplicationMessage.Topic);

            string stringPayload = null;
            JObject messagePayload;

            try
            {
                stringPayload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                messagePayload = JObject.Parse(stringPayload);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not parse MQTT message payload as JSON object. String payload (if successfully decoded): {stringPayload}", stringPayload);
                return;
            }

            foreach (var messageHandler in _messageHandlers)
            {
                try
                {
                    if (messageHandler.AppliesAsync(args.ApplicationMessage.Topic, messagePayload))
                        await messageHandler.HandleAsync(args.ApplicationMessage.Topic, messagePayload);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"An exception occurred while determining whether to apply/handling {messageHandler.GetType()}");
                }
            }
        }

        protected void OnConnectedHandler(MqttClientConnectedEventArgs args)
        {
            _logger.LogInformation($"Connected to MQTT broker, auth result code {args.AuthenticateResult.ResultCode}");
        }

        protected void OnDisconnectedHandler(MqttClientDisconnectedEventArgs args)
        {
            if (args.Exception != null)
                _logger.LogError(args.Exception, "Could not connect to MQTT broker");
            else if (!_cancellationToken.IsCancellationRequested)
                _logger.LogWarning($"Disconnected from event broker, authResultCode={args.AuthenticateResult?.ResultCode}, exc={args.Exception}");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            _logger.LogInformation("Service starting");
            await _client.StartAsync(_options);
            _backgroundTask = Task.Factory.StartNew(() => RefreshSubscriptionsPeriodically(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            _logger.LogInformation("Service started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stopping");
            await _client.StopAsync();
            await _backgroundTask;
            _logger.LogInformation("Service stopped");
        }

        protected async Task RefreshSubscriptionsPeriodically(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await RefreshSubscriptions(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An unexpected error occurred while refreshing subscriptions.");
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }

        protected async Task RefreshSubscriptions(CancellationToken cancellationToken)
        {
            var query = new PagedQuery()
            {
                CurrentPage = 1
            };

            var posIdentifiers = new HashSet<Guid>();
            bool addedAtLeastOne;

            do
            {
                addedAtLeastOne = false;
                var posPage = await _posService.BrowsePointsOfSale(query);

                if (posPage.Items is null)
                    break;

                foreach (var pos in posPage.Items)
                    if (posIdentifiers.Add(pos.Id))
                        addedAtLeastOne = true;
            } while (addedAtLeastOne);

            var subscriptions = posIdentifiers.Select(_posTopicClassifier.GetStatsTopic);
            _logger.LogInformation($"Updating subscriptions to {posIdentifiers.Count} points of sale");

            try
            {
                await _client.SubscribeAsync(subscriptions.Select(topic => new TopicFilterBuilder().WithTopic(topic).Build()));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update subscriptions");
            }
        }
    }
}
