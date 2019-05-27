using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Events.AccountingGroup;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.AccountingGroups.Handlers.AccountingGroup
{
    public class UpdateAccountingGroupHandler : ICommandHandler<IUpdateAccountingGroup, IOperationResult>
    {
        private readonly IAccountingGroupRepository _repository;
        private readonly IBusPublisher _busPublisher;
        private readonly ISaleStrategyVerifier _saleStrategyVerifier;

        public UpdateAccountingGroupHandler(IAccountingGroupRepository repository, IBusPublisher busPublisher, ISaleStrategyVerifier saleStrategyVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _saleStrategyVerifier = saleStrategyVerifier ?? throw new ArgumentNullException(nameof(saleStrategyVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateAccountingGroup command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var accountingGroup = await _repository.GetAsync(command.Id);
            if (accountingGroup is null)
                throw new BaristaException("accounting_group_not_found", $"Could not find accounting group with ID '{command.Id}'");

            await _saleStrategyVerifier.AssertExists(command.SaleStrategyId);

            accountingGroup.SetDisplayName(command.DisplayName);
            accountingGroup.SetSaleStrategyId(command.SaleStrategyId);
            await _repository.UpdateAsync(accountingGroup);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new AccountingGroupUpdated(accountingGroup.Id, accountingGroup.DisplayName, accountingGroup.SaleStrategyId));
            return OperationResult.Ok();
        }
    }
}
