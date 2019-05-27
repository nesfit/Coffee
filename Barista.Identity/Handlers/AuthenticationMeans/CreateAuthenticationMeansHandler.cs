using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Events.AuthenticationMeans;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class CreateAuthenticationMeansHandler : ICommandHandler<ICreateAuthenticationMeans, IIdentifierResult>
    {
        private readonly IAuthenticationMeansRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public CreateAuthenticationMeansHandler(IAuthenticationMeansRepository meansRepository, IBusPublisher busPublisher)
        {
            _repository = meansRepository ?? throw new ArgumentNullException(nameof(meansRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateAuthenticationMeans command, ICorrelationContext context)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var means = new Domain.AuthenticationMeans(command.Id, command.Label, command.Method, command.Value, command.ValidSince, command.ValidUntil);
            await _repository.AddAsync(means);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("authentication_means_already_exists", $"An authentication means with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new AuthenticationMeansCreated(means.Id, means.Label, means.Method, means.ValidSince, means.ValidUntil));

            return new IdentifierResult(means.Id);
        }
    }
}
