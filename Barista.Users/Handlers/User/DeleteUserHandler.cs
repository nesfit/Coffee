using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.User;
using Barista.Users.Events.User;
using Barista.Users.Repositories;

namespace Barista.Users.Handlers.User
{
    public class DeleteUserHandler : ICommandHandler<IDeleteUser, IOperationResult>
    {
        private readonly IUserRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteUserHandler(IUserRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteUser command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var user = await _repository.GetAsync(command.Id);

            if (user is null)
                throw new BaristaException("user_not_found", $"Could not find user with ID '{command.Id}'");
            
            await _repository.DeleteAsync(user);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new UserDeleted(user.Id));
            return OperationResult.Ok();
        }
    }
}
