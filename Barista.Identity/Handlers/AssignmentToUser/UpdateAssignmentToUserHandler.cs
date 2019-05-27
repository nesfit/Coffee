using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Identity.Events.AssignmentToUser;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;

namespace Barista.Identity.Handlers.AssignmentToUser
{
    public class UpdateAssignmentToUserHandler :  ICommandHandler<IUpdateAssignmentToUser, IOperationResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAuthenticationMeansVerifier _meansVerifier;
        private readonly IUserVerifier _userVerifier;
        private readonly IAssignmentExclusivityVerifier _exclVerifier;

        public UpdateAssignmentToUserHandler(IAssignmentsRepository repository, IBusPublisher busPublisher, IAuthenticationMeansVerifier meansVerifier, IUserVerifier userVerifier, IAssignmentExclusivityVerifier exclVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _meansVerifier = meansVerifier ?? throw new ArgumentNullException(nameof(meansVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
            _exclVerifier = exclVerifier ?? throw new ArgumentNullException(nameof(exclVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateAssignmentToUser command, ICorrelationContext context)
        {
            var assignment = await _repository.GetAsync(command.Id);
            if (assignment is null)
                throw new BaristaException("assignment_not_found", $"Assignment with ID '{command.Id}' was not found");
            if (!(assignment is Domain.AssignmentToUser assignmentToUser))
                throw new BaristaException("invalid_assignment_update_command", "This assignment does not assign means to an user and as such cannot be updated with this command.");

            await Task.WhenAll(
                _meansVerifier.AssertExists(command.MeansId),
                _userVerifier.AssertExists(command.UserId)
            );

            assignment.SetMeansId(command.MeansId);
            assignment.SetValidity(command.ValidSince, command.ValidUntil);
            assignmentToUser.SetIsShared(command.IsShared);
            assignmentToUser.SetUserId(command.UserId);

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

            await _busPublisher.Publish(new AssignmentToUserUpdated(assignmentToUser.Id, assignmentToUser.MeansId,
                assignmentToUser.ValidSince, assignmentToUser.ValidUntil, assignmentToUser.UserId,
                assignmentToUser.IsShared));

            return OperationResult.Ok();
        }
    }
}
