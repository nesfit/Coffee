using System;
using System.Threading.Tasks;
using Barista.Common.Dispatchers;
using Barista.Common.OperationResults;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Common.AspNetCore
{
    public abstract class BaristaController : ControllerBase
    {
        protected IDispatcher Dispatcher { get; }

        protected BaristaController(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        protected Task HandleAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand where TResult : IOperationResult
            => Dispatcher.HandleAsync<TCommand, TResult>(command, new CorrelationContext(Guid.Empty, Guid.Empty, Guid.Empty)); // todo

        protected Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => Dispatcher.QueryAsync(query);
    }
}
