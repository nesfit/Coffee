using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common.EfCore
{
    public interface IBrowsableRepository<TEntity>
    {
        Task<IPagedResult<TEntity>> BrowseAsync(IPagedQueryImpl<TEntity> pagedQuery);
    }
}
