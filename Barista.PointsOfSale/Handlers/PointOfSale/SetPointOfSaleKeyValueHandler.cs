using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSale;
using Barista.PointsOfSale.Events.PointOfSale;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class SetPointOfSaleKeyValueHandler : ICommandHandler<ISetPointOfSaleKeyValue, IOperationResult>
    {
        private readonly IPointOfSaleRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public SetPointOfSaleKeyValueHandler(IPointOfSaleRepository pointOfSaleRepository, IBusPublisher busPublisher)
        {
            _repository = pointOfSaleRepository ?? throw new ArgumentNullException(nameof(pointOfSaleRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(ISetPointOfSaleKeyValue command, ICorrelationContext correlationContext)
        {
            var pos = await _repository.GetAsync(command.PointOfSaleId);
            if (pos is null)
                throw new BaristaException("point_of_sale_not_found", $"Could not find point of sale with ID '{command.PointOfSaleId}'");

            pos.SetKeyValue(command.Key, command.Value);
            await _repository.UpdateAsync(pos);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new PointOfSaleKeyValueUpdated(pos.Id, command.Key, command.Value));
            return OperationResult.Ok();
        }
    }
}
