using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Consistency
{
    public interface IEventDispatcher
    {
        Task DispatchEvent(IEvent @event);
    }
}
