using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class Dispatcher : IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public Dispatcher(ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command, ICorrelationContext context) where TCommand : ICommand where TResult : IOperationResult
            => await _commandDispatcher.HandleAsync<TCommand, TResult>(command, context);

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => await _queryDispatcher.QueryAsync<TResult>(query);
    }
}
