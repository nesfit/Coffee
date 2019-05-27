using System;

namespace Barista.Consistency.Activities
{
    public class SourceEventData : ISourceEventData
    {
        public SourceEventData(string fullyQualifiedEventTypeName, string eventData, DateTimeOffset createdAt)
        {
            if (string.IsNullOrWhiteSpace(fullyQualifiedEventTypeName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fullyQualifiedEventTypeName));
            if (string.IsNullOrWhiteSpace(eventData))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(eventData));

            FullyQualifiedEventTypeName = fullyQualifiedEventTypeName;
            EventData = eventData;
            CreatedAt = createdAt;
        }

        public string FullyQualifiedEventTypeName { get; }
        public string EventData { get; }
        public DateTimeOffset CreatedAt { get; }
    }
}
