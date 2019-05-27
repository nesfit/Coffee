using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Barista.Mqtt.MessageHandlers
{
    public interface IMqttMessageHandler
    {
        bool AppliesAsync(string topic, JObject messagePayload);
        Task HandleAsync(string topic, JObject messagePayload);
    }
}
