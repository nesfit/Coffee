using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Events.UserAuthorization;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.UserAuthorization
{
    public class UpdateUserAuthorizationHandler : ICommandHandler<IUpdatePointOfSaleUserAuthorization, IOperationResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;
        public UpdateUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUpdatePointOfSaleUserAuthorization command, ICorrelationContext context)
        {
            if (!Enum.TryParse<UserAuthorizationLevel>(command.Level, out var authLevel))
                throw new BaristaException("invalid_user_authorization_level", $"Unknown user authorization level '{command.Level}'");

            var userAuthorization = await _repository.GetAsync(command.PointOfSaleId, command.UserId);
            if (userAuthorization is null)
                throw new BaristaException("user_authorization_not_found", $"User with ID '{command.UserId}' is not currently authorized to point of sale with ID '{command.PointOfSaleId}");

            userAuthorization.SetLevel(authLevel);

            await _repository.UpdateAsync(userAuthorization);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new UserAuthorizationUpdated(userAuthorization.PointOfSaleId, userAuthorization.UserId, userAuthorization.Level.ToString()));
            return OperationResult.Ok();
        }
    }
}
