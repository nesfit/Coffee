using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;
using Barista.PointsOfSale.Events.UserAuthorization;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.UserAuthorization
{
    public class DeleteUserAuthorizationHandler : ICommandHandler<IDeletePointOfSaleUserAuthorization, IOperationResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeletePointOfSaleUserAuthorization command, ICorrelationContext context)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var userAuthorization = await _repository.GetAsync(command.PointOfSaleId, command.UserId);
            if (userAuthorization is null)
                throw new BaristaException("user_authorization_not_found", $"User with ID '{command.UserId}' is not currently authorized to point of sale with ID '{command.PointOfSaleId}");

            await _repository.DeleteAsync(userAuthorization);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new UserAuthorizationDeleted(userAuthorization.PointOfSaleId, userAuthorization.UserId));
            return OperationResult.Ok();
        }
    }
}
