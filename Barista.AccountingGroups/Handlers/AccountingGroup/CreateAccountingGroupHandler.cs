using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Events.AccountingGroup;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.AccountingGroups.Handlers.AccountingGroup
{
    public class CreateAccountingGroupHandler : ICommandHandler<ICreateAccountingGroup, IIdentifierResult>
    {
        private readonly IAccountingGroupRepository _accGroupRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ISaleStrategyVerifier _saleStrategyVerifier;

        public CreateAccountingGroupHandler(IAccountingGroupRepository accGroupRepository, IBusPublisher busPublisher, ISaleStrategyVerifier saleStrategyVerifier)
        {
            _accGroupRepository = accGroupRepository ?? throw new ArgumentNullException(nameof(accGroupRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _saleStrategyVerifier = saleStrategyVerifier ?? throw new ArgumentNullException(nameof(saleStrategyVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateAccountingGroup command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            await _saleStrategyVerifier.AssertExists(command.SaleStrategyId);

            var accountingGroup = new Domain.AccountingGroup(command.Id, command.DisplayName, command.SaleStrategyId);
            await _accGroupRepository.AddAsync(accountingGroup);

            try
            {
                await _accGroupRepository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("accounting_group_already_exists", $"An accounting group with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new AccountingGroupCreated(command.Id, command.DisplayName, command.SaleStrategyId));

            return new IdentifierResult(accountingGroup.Id);
        }
    }
}
