using System;
using System.Collections.Generic;

namespace Barista.Contracts
{
    public interface IPagedResult
    {
        int CurrentPage { get; }
        int ResultsPerPage { get; }
        int TotalPages { get; }
        int TotalResults { get; }
    }

    public interface IPagedResult<out T> : IPagedResult
    {
        IEnumerable<T> Items { get; }
    }
}
