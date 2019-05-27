using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Events.UserAuthorization;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;

namespace Barista.AccountingGroups.Handlers.UserAuthorization
{
    public class UpdateUserAuthorizationHandler : ICommandHandler<IUpdateAccountingGroupUserAuthorization, IOperationResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public UpdateUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateAccountingGroupUserAuthorization command, ICorrelationContext context)
        {
            if (!Enum.TryParse<UserAuthorizationLevel>(command.Level, out var userAuthLevel))
                throw new BaristaException("invalid_user_auth_level", $"Could not parse user authorization level '{command.Level}'");

            var userAuthorization = await _repository.GetAsync(command.AccountingGroupId, command.UserId);
            if (userAuthorization is null)
                throw new BaristaException("user_authorization_not_found", $"User with ID '{command.UserId}' is not currently authorized to accounting group with ID '{command.AccountingGroupId}");

            userAuthorization.SetLevel(userAuthLevel);

            await _repository.UpdateAsync(userAuthorization);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new UserAuthorizationUpdated(command.AccountingGroupId, command.UserId, command.Level));

            return OperationResult.Ok();
        }
    }
}
