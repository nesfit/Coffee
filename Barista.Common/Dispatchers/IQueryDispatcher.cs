using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
