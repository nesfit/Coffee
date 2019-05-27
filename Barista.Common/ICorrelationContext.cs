using System;

namespace Barista.Common
{
    public interface ICorrelationContext
    {
        Guid MessageId { get; }
        Guid ConversationId { get; }
        Guid CorrelationId { get; }
    }
}
