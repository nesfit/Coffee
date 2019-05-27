using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;

namespace Barista.Common.Dispatchers
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public interface IDispatcher
    {
        Task<TResult> HandleAsync<TCommand, TResult>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand where TResult : IOperationResult;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
