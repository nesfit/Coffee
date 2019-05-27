using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Identity.Events.AssignmentToUser;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;

namespace Barista.Identity.Handlers.AssignmentToUser
{
    public class CreateAssignmentToUserHandler : ICommandHandler<ICreateAssignmentToUser, IIdentifierResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAuthenticationMeansVerifier _meansVerifier;
        private readonly IUserVerifier _userVerifier;
        private readonly IAssignmentExclusivityVerifier _exclVerifier;

        public CreateAssignmentToUserHandler(IAssignmentsRepository repository, IBusPublisher busPublisher, IAuthenticationMeansVerifier meansVerifier, IUserVerifier userVerifier, IAssignmentExclusivityVerifier exclVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _meansVerifier = meansVerifier ?? throw new ArgumentNullException(nameof(meansVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
            _exclVerifier = exclVerifier ?? throw new ArgumentNullException(nameof(exclVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateAssignmentToUser command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _meansVerifier.AssertExists(command.MeansId),
                _userVerifier.AssertExists(command.UserId)
            );

            var assignment = new Domain.AssignmentToUser(command.Id, command.MeansId, command.ValidSince, command.ValidUntil, command.UserId, command.IsShared);
            await _repository.AddAsync(assignment);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("assignment_to_user_already_exists", $"An assignment with the ID '{command.Id}' already exists.");
            }

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

            await _busPublisher.Publish(new AssignmentToUserCreated(assignment.Id, assignment.MeansId, assignment.ValidSince, assignment.ValidUntil, assignment.UserId, assignment.IsShared));

            return new IdentifierResult(command.Id);
        }
    }
}
