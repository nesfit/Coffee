using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.User;
using Barista.Users.Events.User;
using Barista.Users.Repositories;

namespace Barista.Users.Handlers.User
{
    public class CreateUserHandler : ICommandHandler<ICreateUser, IIdentifierResult>
    {
        private readonly IUserRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public CreateUserHandler(IUserRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateUser command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var user = new Domain.User(command.Id, command.FullName, command.EmailAddress, command.IsAdministrator, command.IsActive);
            await _repository.AddAsync(user);


            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("user_already_exists", $"A user with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new UserCreated(user.Id, user.FullName, user.EmailAddress, user.IsAdministrator, user.IsActive));
            return new IdentifierResult(user.Id);
        }
    }
}
