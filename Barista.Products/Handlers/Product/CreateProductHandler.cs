using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.Product;
using Barista.Products.Events;
using Barista.Products.Repositories;

namespace Barista.Products.Handlers.Product
{
    public class CreateProductHandler : ICommandHandler<ICreateProduct, IIdentifierResult>
    {
        private readonly IProductRepository _repository;
        private readonly IBusPublisher _busPublisher;

        public CreateProductHandler(IProductRepository repository, IBusPublisher busPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }

        public async Task<IIdentifierResult> HandleAsync(ICreateProduct command, ICorrelationContext context)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var product = new Domain.Product(command.Id, command.DisplayName, command.RecommendedPrice);
            await _repository.AddAsync(product);

            try
            {
                await _repository.SaveChanges();
            }
            catch (EntityAlreadyExistsException)
            {
                throw new BaristaException("product_already_exists", $"A product with the ID '{command.Id}' already exists.");
            }

            await _busPublisher.Publish(new ProductCreated(product.Id, product.DisplayName, product.RecommendedPrice));
            return new IdentifierResult(product.Id);
        }
    }
}
