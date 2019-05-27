using System;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;

namespace Barista.Common
{
    public interface IBusPublisher
    {
        Task<Guid> Send<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task<IOperationResult> SendRequest<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task<TResult> SendRequest<TCommand, TResult>(TCommand command) where TCommand : class, ICommand where TResult : class, IOperationResult;
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
