using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.Product;
using Barista.Api.Dto;
using Barista.Api.Models.Products;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.MassTransit;
using Barista.Common.OperationResults;
using Barista.Contracts;
using Barista.Contracts.Commands.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : BaseController
    {
        private readonly IProductsService _productsService;

        public ProductsController(IBusPublisher busPublisher, IServiceProvider serviceProvider, IProductsService productsService) : base(busPublisher)
        {
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResult<Product>>> BrowseProducts([FromQuery] DisplayNameQuery query)
            => Collection(await _productsService.BrowseProducts(query));

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(Product))]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
            => Single(await _productsService.GetProduct(id));

        [HttpPost]
        [Authorize(Policies.IsAdvancedUser)]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {
            return await SendAndHandleIdentifierResultCommand(
                new CreateProduct(Guid.NewGuid(), productDto.DisplayName, productDto.RecommendedPrice),
                nameof(GetProduct)
            );
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsAdvancedUser)]
        public async Task<ActionResult> UpdateProduct(Guid id, ProductDto productDto)
        {
            return await SendAndHandleOperationCommand(new UpdateProduct(id, productDto.DisplayName, productDto.RecommendedPrice));
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsAdvancedUser)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            return await SendAndHandleOperationCommand(new DeleteProduct(id));
        }
    }
}
