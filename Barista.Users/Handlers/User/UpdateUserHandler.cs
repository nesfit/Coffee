using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.User;
using Barista.Users.Events.User;
using Barista.Users.Repositories;

namespace Barista.Users.Handlers.User
{
    public class UpdateUserHandler : ICommandHandler<IUpdateUser, IOperationResult>
    {
        private readonly IUserRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public UpdateUserHandler(IUserRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateUser command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var user = await _repository.GetAsync(command.Id);

            if (user is null)
                throw new BaristaException("user_not_found", $"Could not find user with ID '{command.Id}'");

            user.SetFullName(command.FullName);
            user.SetEmailAddress(command.EmailAddress);
            user.SetIsAdministrator(command.IsAdministrator);
            user.SetIsActive(command.IsActive);

            await _repository.UpdateAsync(user);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new UserUpdated(user.Id, user.FullName, user.EmailAddress, user.IsAdministrator, user.IsActive));
            return OperationResult.Ok();
        }
    }
}
