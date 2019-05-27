using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.PointOfSale;
using Barista.PointsOfSale.Events.PointOfSale;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class DeletePointOfSaleHandler : ICommandHandler<IDeletePointOfSale, IOperationResult>
    {
        private readonly IPointOfSaleRepository _pointOfSaleRepository;
        private readonly IBusPublisher _busPublisher;

        public DeletePointOfSaleHandler(IPointOfSaleRepository pointOfSaleRepository, IBusPublisher busPublisher)
        {
            _pointOfSaleRepository = pointOfSaleRepository ?? throw new ArgumentNullException(nameof(pointOfSaleRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeletePointOfSale command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var pos = await _pointOfSaleRepository.GetAsync(command.Id);
            if (pos is null)
                throw new BaristaException("point_of_sale_not_found", $"Could not find point of sale with ID {command.Id}");

            await _pointOfSaleRepository.DeleteAsync(pos);
            await _pointOfSaleRepository.SaveChanges();

            await _busPublisher.Publish(new PointOfSaleDeleted(pos.Id));

            return OperationResult.Ok();
        }
    }
}
