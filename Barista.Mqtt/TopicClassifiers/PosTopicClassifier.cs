using System;
using System.Text.RegularExpressions;

namespace Barista.Mqtt.TopicClassifiers
{
    public class PosTopicClassifier : IPosTopicClassifier
    {
        private static readonly Regex PosTopicRegex = new Regex("^pos/([a-f0-9]+)/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool IsMatch(string topic) => PosTopicRegex.IsMatch(topic);
        public Guid GetPointOfSaleId(string topic) => Guid.Parse(PosTopicRegex.Match(topic).Groups[1].Value);
        public string GetCommandsTopic(Guid posId) => $"pos/{posId:N}/cmd";
        public string GetStatsTopic(Guid posId) => $"pos/{posId:N}/stat";
    }
}
