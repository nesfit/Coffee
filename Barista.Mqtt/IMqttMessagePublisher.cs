using System.Threading.Tasks;

namespace Barista.Mqtt
{
    public interface IMqttMessagePublisher
    {
        Task Publish<T>(string topic, T message);
    }
}
