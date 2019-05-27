using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Identity.Events.AssignmentToPointOfSale;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AssignmentToPointOfSale
{
    public class DeleteAssignmentToPointOfSaleHandler : ICommandHandler<IDeleteAssignmentToPointOfSale, IOperationResult>
    {
        private readonly IAssignmentsRepository _assignmentsRepository;
        private readonly IBusPublisher _busPublisher;

        public DeleteAssignmentToPointOfSaleHandler(IAssignmentsRepository assignmentsRepository, IBusPublisher busPublisher)
        {
            _assignmentsRepository = assignmentsRepository ?? throw new ArgumentNullException(nameof(assignmentsRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteAssignmentToPointOfSale command, ICorrelationContext context)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var assignment = await _assignmentsRepository.GetAsync(command.Id);
            if (assignment is null)
                throw new BaristaException("assignment_not_found", $"Assignment with ID '{command.Id}' was not found");
            if (!(assignment is Domain.AssignmentToPointOfSale))
                throw new BaristaException("invalid_command", $"The assignment with ID '{command.Id}' is not an assignment to point of sale and as such cannot be deleted with this command.");

            await _assignmentsRepository.DeleteAsync(assignment);
            await _assignmentsRepository.SaveChanges();
            await _busPublisher.Publish(new AssignmentToPointOfSaleDeleted(command.Id));

            return OperationResult.Ok();
        }
    }
}
