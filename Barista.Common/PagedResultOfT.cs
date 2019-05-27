using System;
using System.Collections.Generic;
using System.Linq;
using Barista.Contracts;

namespace Barista.Common
{
    public class PagedResult<T> : IPagedResult<T>
    {
        private readonly IPagedResult _paginationData;

        public PagedResult(IPagedResult paginationData, IEnumerable<T> items)
        {
            _paginationData = paginationData ?? throw new ArgumentNullException((nameof(paginationData)));
            Items = items ?? Enumerable.Empty<T>();
        }

        public int CurrentPage => _paginationData.CurrentPage;
        public int ResultsPerPage => _paginationData.ResultsPerPage;
        public int TotalPages => _paginationData.TotalPages;
        public int TotalResults => _paginationData.TotalResults;
        public IEnumerable<T> Items { get; }

        public IPagedResult<TOther> MapItems<TOther>(Func<T, TOther> mapFunc)
        {
            return new PagedResult<TOther>(this, Items.Select(mapFunc));
        }
    }
}
