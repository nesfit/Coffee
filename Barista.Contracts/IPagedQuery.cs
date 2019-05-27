using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Contracts
{
    public interface IPagedQuery : IQuery
    {
        int CurrentPage { get; }
        int ResultsPerPage { get; }
        string[] SortBy { get; }
    }

    public interface IPagedQuery<T> : IQuery<IPagedResult<T>>, IPagedQuery
    {
    }
}
