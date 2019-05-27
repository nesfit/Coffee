using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SaleStrategy;
using Barista.SaleStrategies.Repositories;
using Barista.SaleStrategies.Services;

namespace Barista.SaleStrategies.Handlers.SaleStrategy
{
    public class ApplySaleStrategyHandler : ICommandHandler<IApplySaleStrategy, IOperationResult>
    {
        private readonly ISaleStrategyRepository _repository;
        private readonly IAccountingService _accountingService;

        public ApplySaleStrategyHandler(ISaleStrategyRepository repository, IAccountingService accountingService)
        {
            _repository = repository;
            _accountingService = accountingService;
        }

        public async Task<IOperationResult> HandleAsync(IApplySaleStrategy command, ICorrelationContext correlationContext)
        {
            var saleStrategy = await _repository.GetSaleStrategy(command.SaleStrategyId);
            if (saleStrategy is null)
                return new OperationResult("sale_strategy_not_found", $"Could not find sale strategy with ID '{command.SaleStrategyId}'");

            var applicationSuccessful = await saleStrategy.ApplyAsync(_accountingService, command.UserId, command.Cost);
            if (applicationSuccessful)
                return OperationResult.Ok();
            else
                return new OperationResult("sale_strategy_application_failed", $"The '{saleStrategy.DisplayName}' rejected the sale attempt.");
        }
    }
}
