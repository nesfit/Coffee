using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSale;
using Barista.PointsOfSale.Events.PointOfSale;
using Barista.PointsOfSale.Repositories;
using Barista.PointsOfSale.Verifiers;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class CreatePointOfSaleHandler : ICommandHandler<ICreatePointOfSale, IIdentifierResult>
    {
        private readonly IPointOfSaleRepository _posRepository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAccountingGroupVerifier _agVerifier;
        private readonly ISaleStrategyVerifier _ssVerifier;

        public CreatePointOfSaleHandler(IPointOfSaleRepository posRepository, IBusPublisher busPublisher, IAccountingGroupVerifier agVerifier, ISaleStrategyVerifier ssVerifier)
        {
            _posRepository = posRepository ?? throw new ArgumentNullException(nameof(posRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _agVerifier = agVerifier ?? throw new ArgumentNullException(nameof(agVerifier));
            _ssVerifier = ssVerifier ?? throw new ArgumentNullException(nameof(ssVerifier));
        }
        
        public async Task<IIdentifierResult> HandleAsync(ICreatePointOfSale command, ICorrelationContext context)
        {
            await Task.WhenAll(
                _agVerifier.AssertExists(command.ParentAccountingGroupId),
                command.SaleStrategyId != null ? _ssVerifier.AssertExists(command.SaleStrategyId.Value) : Task.CompletedTask
            );
        
            var pointOfSale = new Domain.PointOfSale(command.Id, command.DisplayName, command.ParentAccountingGroupId, command.SaleStrategyId, command.Features ?? new string[0]);
            await _posRepository.AddAsync(pointOfSale);

            try
            {
                await _posRepository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("point_of_sale_already_exists", $"A point of sale with the ID '{command.Id}' already exists.");
            }


            await _busPublisher.Publish(new PointOfSaleCreated(pointOfSale.Id, pointOfSale.DisplayName, pointOfSale.ParentAccountingGroupId, pointOfSale.SaleStrategyId));

            return new IdentifierResult(pointOfSale.Id);
        }
    }
}
