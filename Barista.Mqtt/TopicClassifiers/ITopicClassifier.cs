namespace Barista.Mqtt.TopicClassifiers
{
    public interface ITopicClassifier
    {
        bool IsMatch(string topic);
    }
}
