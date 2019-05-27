using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common
{
    public class ClientsidePaginator<T> : IClientsidePaginator<T>
    {
        public Task<IPagedResult<T>> PaginateAsync(ICollection<T> sourceCollection, IPagedQueryImpl<T> query)
        {
            return sourceCollection.PaginateClientsideAsync(query);
        }
    }
}
