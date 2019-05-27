using System;
using System.Threading.Tasks;
using Barista.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            return await handler.HandleAsync((dynamic)query);
        }
    }
}
