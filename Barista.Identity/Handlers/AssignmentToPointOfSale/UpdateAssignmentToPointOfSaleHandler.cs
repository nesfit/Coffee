using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Identity.Events.AssignmentToPointOfSale;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;

namespace Barista.Identity.Handlers.AssignmentToPointOfSale
{
    public class UpdateAssignmentToPointOfSaleHandler : ICommandHandler<IUpdateAssignmentToPointOfSale, IOperationResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAuthenticationMeansVerifier _meansVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IAssignmentExclusivityVerifier _exclVerifier;

        public UpdateAssignmentToPointOfSaleHandler(IAssignmentsRepository repository, IBusPublisher busPublisher, IAuthenticationMeansVerifier meansVerifier, IPointOfSaleVerifier posVerifier, IAssignmentExclusivityVerifier exclVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _meansVerifier = meansVerifier ?? throw new ArgumentNullException(nameof(meansVerifier));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _exclVerifier = exclVerifier ?? throw new ArgumentNullException(nameof(exclVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateAssignmentToPointOfSale command, ICorrelationContext context)
        {
            var assignment = await _repository.GetAsync(command.Id);
            if (assignment is null)
                throw new BaristaException("assignment_not_found", $"Assignment with ID '{command.Id}' was not found");
            if (!(assignment is Domain.AssignmentToPointOfSale assignmentToUser))
                throw new BaristaException("invalid_assignment_update_command", "This assignment does not assign means to a point of sale and as such cannot be updated with this command.");

            await Task.WhenAll(
                _meansVerifier.AssertExists(command.MeansId),
                _posVerifier.AssertExists(command.PointOfSaleId)
            );

            assignment.SetMeansId(command.MeansId);
            assignment.SetValidity(command.ValidSince, command.ValidUntil);
            assignmentToUser.SetPointOfSale(command.PointOfSaleId);

            await _repository.UpdateAsync(assignmentToUser);
            await _repository.SaveChanges();

            try
            {
                await _exclVerifier.VerifyAssignmentExclusivity(command.MeansId);
            }
            catch (BaristaException)
            {
                await _repository.DeleteAsync(assignment);
                await _repository.SaveChanges();
                throw;
            }

            await _busPublisher.Publish(new AssignmentToPointOfSaleUpdated(assignmentToUser.Id,
                assignmentToUser.MeansId, assignmentToUser.ValidSince, assignmentToUser.ValidUntil,
                assignmentToUser.PointOfSaleId));

            return OperationResult.Ok();
        }
    }
}
