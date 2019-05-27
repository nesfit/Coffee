using System;
using System.Threading.Tasks;
using Barista.AccountingGroups.Events.UserAuthorization;
using Barista.AccountingGroups.Repositories;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AccountingGroupUserAuthorization;

namespace Barista.AccountingGroups.Handlers.UserAuthorization
{
    public class DeleteUserAuthorizationHandler : ICommandHandler<IDeleteAccountingGroupUserAuthorization, IOperationResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteAccountingGroupUserAuthorization command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var userAuthorization = await _repository.GetAsync(command.AccountingGroupId, command.UserId);
            if (userAuthorization is null)
                throw new BaristaException("user_authorization_not_found", $"User with ID '{command.UserId}' is not currently authorized to accounting group with ID '{command.AccountingGroupId}");

            await _repository.DeleteAsync(userAuthorization);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new UserAuthorizationDeleted(userAuthorization.AccountingGroupId, userAuthorization.UserId));

            return OperationResult.Ok();
        }
    }
}
