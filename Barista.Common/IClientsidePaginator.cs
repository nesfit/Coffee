using System.Collections.Generic;
using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common
{
    public interface IClientsidePaginator<T>
    {
        Task<IPagedResult<T>> PaginateAsync(ICollection<T> sourceCollection, IPagedQueryImpl<T> query);
    }
}