using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace Barista.Consistency.Activities
{
    public abstract class ConsistencyRemediationActivity<TParams, TChildId> : ExecuteActivity<TParams> where TParams : class, IHasSourceEventData, IConsistencyRemediationVariables
    {
        protected ConsistencyRemediationActivity(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private readonly ILogger _logger;
        protected abstract Task<IEnumerable<TChildId>> FindAsync(TParams args);
        protected abstract Task<IOperationResult> RemedyAsync(TParams args, TChildId inconsistentEntityId);

        public async Task<ExecutionResult> Execute(ExecuteContext<TParams> context)
        {
            var inconsistenciesIds = await FindAsync(context.Arguments);
            var anotherRunRequired = false;

            if (inconsistenciesIds != null)
            {
                foreach (var inconsistentId in inconsistenciesIds)
                {
                    anotherRunRequired = true;

                    try
                    {
                        await RemedyAsync(context.Arguments, inconsistentId);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Could not remedy inconsistency with ID {inconsistentId}");
                        return context.Faulted(e);
                    }
                }
            }

            var addition = anotherRunRequired ? 1 : 0;
            return context.CompletedWithVariables(new ConsistencyRemediationVariables
                {RerunRequiredTimes = context.Arguments.RerunRequiredTimes + addition}
            );
        }
    }
}
