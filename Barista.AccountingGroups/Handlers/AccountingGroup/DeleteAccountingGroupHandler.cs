using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Events.AccountingGroup;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroup;

namespace Barista.AccountingGroups.Handlers.AccountingGroup
{
    public class DeleteAccountingGroupHandler : ICommandHandler<IDeleteAccountingGroup, IOperationResult>
    {
        private readonly IAccountingGroupRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteAccountingGroupHandler(IAccountingGroupRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteAccountingGroup command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var accountingGroup = await _repository.GetAsync(command.Id);
            if (accountingGroup is null)
                throw new BaristaException("accounting_group_not_found", $"Could not find accounting group with ID {command.Id}");

            await _repository.DeleteAsync(accountingGroup);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new AccountingGroupDeleted(command.Id));

            return OperationResult.Ok();
        }
    }
}
