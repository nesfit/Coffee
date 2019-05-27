using System;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command, ICorrelationContext correlationContext) where TCommand : ICommand where TResult : IOperationResult
            => await ((ICommandHandler<TCommand, TResult>) _serviceProvider.GetService(typeof(ICommandHandler<TCommand,TResult>))).HandleAsync(command, correlationContext);
    }
}
