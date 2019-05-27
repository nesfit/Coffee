using System;

namespace Barista.Consistency.Activities
{
    public interface ISourceEventData
    {
        string FullyQualifiedEventTypeName { get; }
        string EventData { get; }
        DateTimeOffset CreatedAt { get; }
    }
}
