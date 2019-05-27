using System.Threading.Tasks;
using Consul;

namespace Barista.Common.Consul
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}