using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Identity.Events.AuthenticationMeans;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class UpdateAuthenticationMeansValueHandler : ICommandHandler<IUpdateAuthenticationMeansValue, IOperationResult>
    {
        private readonly IAuthenticationMeansRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public UpdateAuthenticationMeansValueHandler(IAuthenticationMeansRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateAuthenticationMeansValue command, ICorrelationContext correlationContext)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var means = await _repository.GetAsync(command.Id);
            if (means is null)
                throw new BaristaException("means_not_found", $"Authentication means with ID '{command.Id}' was not found");

            means.SetValue(command.Value);
            await _repository.UpdateAsync(means);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new AuthenticationMeansUpdated(means.Id, means.Label, means.Method, means.ValidSince, means.ValidUntil));
            return OperationResult.Ok();
        }
    }
}
