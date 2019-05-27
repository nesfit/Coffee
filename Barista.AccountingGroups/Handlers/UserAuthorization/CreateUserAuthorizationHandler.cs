using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Events.UserAuthorization;
using Barista.AccountingGroups.Repositories;
using Barista.AccountingGroups.Verifiers;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;

namespace Barista.AccountingGroups.Handlers.UserAuthorization
{
    public class CreateUserAuthorizationHandler : ICommandHandler<ICreateAccountingGroupUserAuthorization, IParentChildIdentifierResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAccountingGroupVerifier _agVerifier;
        private readonly IUserVerifier _userVerifier;

        public CreateUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher, IAccountingGroupVerifier agVerifier, IUserVerifier userVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _agVerifier = agVerifier ?? throw new ArgumentNullException(nameof(agVerifier));
            _userVerifier = userVerifier ?? throw new ArgumentNullException(nameof(userVerifier));
        }

        public async Task<IParentChildIdentifierResult> HandleAsync(ICreateAccountingGroupUserAuthorization command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!Enum.TryParse<UserAuthorizationLevel>(command.Level, out var userAuthLevel))
                throw new BaristaException("invalid_user_auth_level", $"Could not parse user authorization level '{command.Level}'");

            await Task.WhenAll(
                _userVerifier.AssertExists(command.UserId),
                _agVerifier.AssertExists(command.AccountingGroupId)
            );
            
            var authorizedUser = new Domain.UserAuthorization(command.AccountingGroupId, command.UserId, userAuthLevel);
            await _repository.AddAsync(authorizedUser);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("accounting_group_user_authorization_already_exists", $"User with ID '{command.AccountingGroupId}' is already an authorized user to accounting group with ID '{command.UserId}'.");
            }

            await _busPublisher.Publish(new UserAuthorizationCreated(command.AccountingGroupId, command.UserId, command.Level));
            return new ParentChildIdentifierResult(command.AccountingGroupId, authorizedUser.UserId);
        }
    }
}
