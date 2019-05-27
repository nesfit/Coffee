using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSale;
using Barista.PointsOfSale.Events.PointOfSale;
using Barista.PointsOfSale.Repositories;
using Barista.PointsOfSale.Verifiers;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class UpdatePointOfSaleHandler : ICommandHandler<IUpdatePointOfSale, IOperationResult>
    {
        private readonly IPointOfSaleRepository _repository;
        private readonly IBusPublisher _busPublisher;

        private readonly IAccountingGroupVerifier _agVerifier;
        private readonly ISaleStrategyVerifier _ssVerifier;

        public UpdatePointOfSaleHandler(IPointOfSaleRepository repository, IBusPublisher busPublisher, IAccountingGroupVerifier agVerifier, ISaleStrategyVerifier ssVerifier)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
            _agVerifier = agVerifier ?? throw new ArgumentNullException(nameof(agVerifier));
            _ssVerifier = ssVerifier ?? throw new ArgumentNullException(nameof(ssVerifier));
        }

        public async Task<IOperationResult> HandleAsync(IUpdatePointOfSale command, ICorrelationContext context)
        {
            var pos = await _repository.GetAsync(command.Id);
            if (pos is null)
                throw new BaristaException("point_of_sale_not_found", $"Could not find point of sale with ID '{command.Id}'");

            await Task.WhenAll(
                _agVerifier.AssertExists(command.ParentAccountingGroupId),
                command.SaleStrategyId != null ? _ssVerifier.AssertExists(command.SaleStrategyId.Value) : Task.CompletedTask
            );

            pos.SetDisplayName(command.DisplayName);
            pos.SetSaleStrategyId(command.SaleStrategyId);
            pos.SetParentAccountingGroupId(command.ParentAccountingGroupId);
            pos.SetFeatures(command.Features ?? new string[0]);
            await _repository.UpdateAsync(pos);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new PointOfSaleUpdated(pos.Id, pos.DisplayName, pos.ParentAccountingGroupId, pos.SaleStrategyId));
            return OperationResult.Ok();
        }
    }
}
