using System;
using Barista.Common;

namespace Barista.Consistency.Domain
{
    public class ScheduledEvent : Entity
    {
        public ScheduledEvent(Guid id, string messageTypeName, string serializedContents, DateTimeOffset scheduledFor) : base(id)
        {
            SetMessageType(messageTypeName);
            SetSerializedContents(serializedContents);
            SetScheduledFor(scheduledFor);
        }

        public string MessageTypeName { get; protected set; }
        public string SerializedContents { get; protected set; }
        public DateTimeOffset ScheduledFor { get; protected set; }

        protected void SetMessageType(string messageTypeName)
        {
            if (string.IsNullOrWhiteSpace(messageTypeName))
                throw new BaristaException("invalid_message_type_name", "Message type name cannot be empty.");
            else if (Type.GetType(messageTypeName) is Type messageType && !messageType.IsInterface)
                throw new BaristaException("invalid_message_type_name", "Message type name cannot reference a non-interface type.");

            MessageTypeName = messageTypeName;
        }

        protected void SetSerializedContents(string serializedContents)
        {
            if (string.IsNullOrWhiteSpace(serializedContents))
                throw new BaristaException("invalid_serialized_contents", "Serialized contents cannot be empty.");

            SerializedContents = serializedContents;
        }

        protected void SetScheduledFor(DateTimeOffset scheduledFor)
        {
            ScheduledFor = scheduledFor;
        }
    }
}
