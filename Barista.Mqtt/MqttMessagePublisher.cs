using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace Barista.Mqtt
{
    public class MqttMessagePublisher : IMqttMessagePublisher
    {
        private readonly IManagedMqttClient _client;

        public MqttMessagePublisher(IManagedMqttClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task Publish<T>(string topic, T message)
        {
            var payload = JsonConvert.SerializeObject(message);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            await _client.PublishAsync(new MqttApplicationMessage()
            {
                ContentType = "application/json",
                Topic = topic,
                Payload = payloadBytes
            });
        }
    }
}
