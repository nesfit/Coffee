using System;
using System.Threading.Tasks;
using Barista.Common.OperationResults;
using Barista.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Barista.Common.MassTransit
{
    public class CommandHandlerWrapper<TCommand, TResult> : IConsumer<TCommand>
        where TCommand : class, ICommand
        where TResult : class, IOperationResult
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Consume(ConsumeContext<TCommand> context)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
                    var result = await handler.HandleAsync(context.Message, context.ToCorrelationContext());
                    await context.RespondAsync(result);
                }
            }
            catch (BaristaException e) when (typeof(TResult) == typeof(IOperationResult))
            {
                await context.RespondAsync(new OperationResult(e.Code, e.Message));
            }
            catch (BaristaException e) when (typeof(TResult) == typeof(IIdentifierResult))
            {
                await context.RespondAsync(new IdentifierResult(e.Code, e.Message));
            }
            catch (BaristaException e) when (typeof(TResult) == typeof(ILongRunningOperationResult))
            {
                await context.RespondAsync(new LongRunningOperationResult(e.Code, e.Message));
            }
            catch (BaristaException e) when (typeof(TResult) == typeof(IParentChildIdentifierResult))
            {
                await context.RespondAsync(new ParentChildIdentifierResult(e.Code, e.Message));
            }
            catch (BaristaException e) when (typeof(TResult) == typeof(IUserIdResolutionResult))
            {
                await context.RespondAsync(new UserIdResolutionResult(e.Code, e.Message));
            }
        }
    }
}