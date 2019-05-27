using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.SpendingLimit;
using Barista.Identity.Events.SpendingLimit;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.SpendingLimit
{
    public class DeleteSpendingLimitHandler : SpendingLimitHandlerBase, ICommandHandler<IDeleteSpendingLimit, IOperationResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteSpendingLimitHandler(IAssignmentsRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteSpendingLimit command, ICorrelationContext context)
        {
            var assignmentToUser = await GetAssignmentToUser(_repository, command.ParentUserAssignmentId);
            var spendingLimit = assignmentToUser.SpendingLimits.FirstOrDefault(sl => sl.Id == command.Id);
            if (spendingLimit is null)
                throw new BaristaException("spending_limit_not_found", $"Spending limit with ID '{command.Id}' was not found in assignment '{command.ParentUserAssignmentId}");

            assignmentToUser.SpendingLimits.Remove(spendingLimit);

            await _repository.UpdateAsync(assignmentToUser);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new SpendingLimitDeleted(spendingLimit.Id, assignmentToUser.Id));
            return OperationResult.Ok();
        }
    }
}
