using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Product;
using Barista.Products.Events;
using Barista.Products.Repositories;

namespace Barista.Products.Handlers.Product
{
    public class DeleteProductHandler : ICommandHandler<IDeleteProduct, IOperationResult>
    {
        private readonly IProductRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public DeleteProductHandler(IProductRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IDeleteProduct command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var product = await _repository.GetAsync(command.Id);
            if (product is null)
                throw new BaristaException("product_not_found", $"Could not find product with ID '{command.Id}'");

            await _repository.DeleteAsync(product);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new ProductDeleted(product.Id));
            return OperationResult.Ok();
        }
    }
}
