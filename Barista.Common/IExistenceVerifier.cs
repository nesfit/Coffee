using System.Threading.Tasks;

namespace Barista.Common
{
    public interface IExistenceVerifier<in TEntityId>
    {
        Task AssertExists(TEntityId entityId);
    }
}
