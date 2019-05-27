using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Identity.Events.AssignmentToPointOfSale;
using Barista.Identity.Repositories;
using Barista.Identity.Verifiers;

namespace Barista.Identity.Handlers.AssignmentToPointOfSale
{
    public class CreateAssignmentToPointOfSaleHandler : ICommandHandler<ICreateAssignmentToPointOfSale, IIdentifierResult>
    {
        private readonly IAssignmentsRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAuthenticationMeansVerifier _meansVerifier;
        private readonly IPointOfSaleVerifier _posVerifier;
        private readonly IAssignmentExclusivityVerifier _exclVerifier;

        public CreateAssignmentToPointOfSaleHandler(IAssignmentsRepository repository, IBusPublisher busPublisher, IAuthenticationMeansVerifier meansVerifier, IPointOfSaleVerifier posVerifier, IAssignmentExclusivityVerifier exclVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _meansVerifier = meansVerifier ?? throw new ArgumentNullException(nameof(meansVerifier));
            _posVerifier = posVerifier ?? throw new ArgumentNullException(nameof(posVerifier));
            _exclVerifier = exclVerifier ?? throw new ArgumentNullException(nameof(exclVerifier));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateAssignmentToPointOfSale command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _meansVerifier.AssertExists(command.MeansId),
                _posVerifier.AssertExists(command.PointOfSaleId)
            );

            var assignment = new Domain.AssignmentToPointOfSale(command.Id, command.MeansId, command.ValidSince, command.ValidUntil, command.PointOfSaleId);
            await _repository.AddAsync(assignment);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("assignment_to_point_of_sale_already_exists", $"An assignment with the ID '{command.Id}' already exists.");
            }

            try
            {
                await _exclVerifier.VerifyAssignmentExclusivity(command.MeansId);
            }
            catch (BaristaException)
            {
                await _repository.DeleteAsync(assignment);
                await _repository.SaveChanges();
                throw;
            }

            await _busPublisher.Publish(new AssignmentToPointOfSaleCreated(command.Id, command.MeansId, command.ValidSince, command.ValidUntil, command.PointOfSaleId));
            return new IdentifierResult(assignment.Id);
        }
    }
}
