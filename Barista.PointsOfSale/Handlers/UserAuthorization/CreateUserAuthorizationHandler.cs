using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;
using Barista.PointsOfSale.Domain;
using Barista.PointsOfSale.Events.UserAuthorization;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.UserAuthorization
{
    public class CreateUserAuthorizationHandler : ICommandHandler<ICreatePointOfSaleUserAuthorization, IParentChildIdentifierResult>
    {
        private readonly IUserAuthorizationRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public CreateUserAuthorizationHandler(IUserAuthorizationRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IParentChildIdentifierResult> HandleAsync(ICreatePointOfSaleUserAuthorization command, ICorrelationContext context)
        {
            if (!Enum.TryParse<UserAuthorizationLevel>(command.Level, out var authLevel))
                throw new BaristaException("invalid_user_authorization_level", $"Unknown user authorization level '{command.Level}'");
            
            var authorizedUser = new Domain.UserAuthorization(command.PointOfSaleId, command.UserId, authLevel);
            await _repository.AddAsync(authorizedUser);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("point_of_sale_authorized_user_already_exists", $"The user with ID '{command.UserId}' is already an authorized user to point of sale with ID '{command.PointOfSaleId}'.");
            }

            await _busPublisher.Publish(new UserAuthorizationCreated(authorizedUser.PointOfSaleId, authorizedUser.UserId, authorizedUser.Level.ToString()));
            return new ParentChildIdentifierResult(authorizedUser.PointOfSaleId, authorizedUser.UserId);
        }
    }
}
