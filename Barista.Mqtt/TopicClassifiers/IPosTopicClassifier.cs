using System;

namespace Barista.Mqtt.TopicClassifiers
{
    public interface IPosTopicClassifier : ITopicClassifier
    {
        Guid GetPointOfSaleId(string topic);
        string GetCommandsTopic(Guid posId);
        string GetStatsTopic(Guid posId);
    }
}
