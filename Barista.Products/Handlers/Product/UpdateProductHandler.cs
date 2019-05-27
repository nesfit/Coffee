using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Product;
using Barista.Products.Events;
using Barista.Products.Repositories;

namespace Barista.Products.Handlers.Product
{
    public class UpdateProductHandler : ICommandHandler<IUpdateProduct, IOperationResult>
    {
        private readonly IProductRepository _repository;
        private readonly IBusPublisher _busPublisher;
        public UpdateProductHandler(IProductRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IOperationResult> HandleAsync(IUpdateProduct command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var product = await _repository.GetAsync(command.Id);
            if (product is null)
                throw new BaristaException("product_not_found", $"Could not find product with ID '{command.Id}'");

            product.SetDisplayName(command.DisplayName);
            product.SetRecommendedPrice(command.RecommendedPrice);
            await _repository.UpdateAsync(product);
            await _repository.SaveChanges();

            await _busPublisher.Publish(new ProductUpdated(product.Id, product.DisplayName, product.RecommendedPrice));
            return OperationResult.Ok();
        }
    }
}
