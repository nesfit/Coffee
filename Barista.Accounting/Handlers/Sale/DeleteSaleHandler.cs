using System;
using System.Threading.Tasks;
using Barista.Accounting.Events.Sale;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Sale;

namespace Barista.Accounting.Handlers.Sale
{
    public class DeleteSaleHandler : ICommandHandler<IDeleteSale, IOperationResult>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IBusPublisher _busPublisher;

        public DeleteSaleHandler(ISalesRepository salesRepository, IBusPublisher busPublisher)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteSale command, ICorrelationContext context)
        {
            var sale = await _salesRepository.GetAsync(command.Id);
            if (sale is null)
                throw new BaristaException("sale_not_found", $"Could not find sale with ID '{command.Id}'");

            await _salesRepository.DeleteAsync(sale);
            await _salesRepository.SaveChanges();
            await _busPublisher.Publish(new SaleDeleted(command.Id));
            return OperationResult.Ok();
        }
    }
}
