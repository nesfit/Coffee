using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SpendingLimit;
using Barista.Identity.Events.SpendingLimit;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.SpendingLimit
{
    public class CreateSpendingLimitHandler : SpendingLimitHandlerBase, ICommandHandler<ICreateSpendingLimit, IIdentifierResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public CreateSpendingLimitHandler(IAssignmentsRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateSpendingLimit command, ICorrelationContext context)
        {
            var assignmentToUser = await GetAssignmentToUser(_repository, command.ParentUserAssignmentId);
            var spendingLimit = new Domain.SpendingLimit(command.Id, command.Interval, command.Value);
            assignmentToUser.SpendingLimits.Add(spendingLimit);

            await _repository.UpdateAsync(assignmentToUser);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("spending_limit_already_exists", $"A spending limit with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new SpendingLimitCreated(spendingLimit.Id, assignmentToUser.Id, spendingLimit.Interval, spendingLimit.Value));
            return new IdentifierResult(spendingLimit.Id);
        }
    }
}
