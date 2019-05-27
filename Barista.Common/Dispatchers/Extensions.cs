using System;
using System.Threading.Tasks;
using Barista.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public static class Extensions
    {
        public static void AddDispatchers(this IServiceCollection builder)
        {
            builder.AddScoped<ICommandDispatcher, CommandDispatcher>();
            builder.AddScoped<IDispatcher, Dispatcher>();
            builder.AddScoped<IQueryDispatcher, QueryDispatcher>();
        }


        public static async Task<bool> QueryAllPages<T>(this IQueryDispatcher queryDispatcher, IPagedQuery<T> query, Func<T, bool> callback)
        {
            IPagedResult<T> page;

            do
            {
                page = await queryDispatcher.QueryAsync(query);

                foreach (var item in page.Items)
                    if (callback(item))
                        return true;

                page.Bind(p => p.CurrentPage, page.CurrentPage + 1); // todo yuck
            } while (page.CurrentPage < page.TotalPages);

            return false;
        }
    }
}
