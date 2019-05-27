using System.Linq;
using Barista.Contracts;

namespace Barista.Common
{
    public interface IPagedQueryImpl<T> : IPagedQuery
    {
        IQueryable<T> Apply(IQueryable<T> q);
    }
}
