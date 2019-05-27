using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext correlationContext);
    }
}
