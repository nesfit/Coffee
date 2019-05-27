using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Barista.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Barista.Common
{
    public static class PaginationExtensions
    {
        public static async Task<IPagedResult<T>> PaginateAsync<T>(this IQueryable<T> queryable, IPagedQueryImpl<T> pagedQuery)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));

            var pageNo = pagedQuery.CurrentPage;
            if (pageNo < 1)
                pageNo = 1;

            var resultsPerPage = pagedQuery.ResultsPerPage;
            if (resultsPerPage < 1)
                resultsPerPage = 10;

            queryable = pagedQuery.Apply(queryable);

            if (await queryable.AnyAsync() == false)
                return new PagedResult<T>(new PagedResult(pageNo, resultsPerPage, 0, 0), null);

            var resultCount = await queryable.CountAsync();
            var paginationData = new PagedResult(pageNo, resultsPerPage, (int)Math.Ceiling(1d * resultCount / resultsPerPage), resultCount);
            var items = await queryable.Skip(resultsPerPage * (pageNo - 1)).Take(resultsPerPage).ToListAsync();
            return new PagedResult<T>(paginationData, items);
        }

        internal static Task<IPagedResult<T>> PaginateClientsideAsync<T>(this ICollection<T> collection, IPagedQueryImpl<T> pagedQuery)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (pagedQuery == null) throw new ArgumentNullException(nameof(pagedQuery));

            var pageNo = pagedQuery.CurrentPage;
            if (pageNo < 1)
                pageNo = 1;

            var resultsPerPage = pagedQuery.ResultsPerPage;
            if (resultsPerPage < 1)
                resultsPerPage = 10;

            var items = pagedQuery.Apply(collection.AsQueryable());

            if (items.Any() == false)
                return Task.FromResult<IPagedResult<T>>(new PagedResult<T>(new PagedResult(pageNo, resultsPerPage, 0, 0), null));

            var resultCount = items.Count();
            var paginationData = new PagedResult(pageNo, resultsPerPage, (int)Math.Ceiling(1d * resultCount / resultsPerPage), resultCount);
            items = items.Skip(resultsPerPage * (pageNo - 1)).Take(resultsPerPage);
            return Task.FromResult<IPagedResult<T>>(new PagedResult<T>(paginationData, items));
        }
    }
}
