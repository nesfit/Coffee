using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Events.AuthenticationMeans;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class DeleteAuthenticationMeansHandler : ICommandHandler<IDeleteAuthenticationMeans, IOperationResult>
    {
        private readonly IAuthenticationMeansRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteAuthenticationMeansHandler(IAuthenticationMeansRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteAuthenticationMeans command, ICorrelationContext context)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));
            if (!(await _repository.GetAsync(command.Id) is Domain.AuthenticationMeans means))
                throw new BaristaException("means_not_found", $"Authentication means with ID '{command.Id}' was not found");

            await _repository.DeleteAsync(means);
            await _repository.SaveChanges();
            await _busPublisher.Publish(new AuthenticationMeansDeleted(command.Id));

            return OperationResult.Ok();
        }
    }
}
