using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;

namespace Barista.Common.Dispatchers
{
    // Based on (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public interface ICommandDispatcher
    {
        Task<TResult> HandleAsync<TCommand, TResult>(TCommand command, ICorrelationContext correlationContext)
            where TCommand : ICommand where TResult : IOperationResult;
    }
}
