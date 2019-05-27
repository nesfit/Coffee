using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Products.Dto;
using Barista.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Products.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : BaristaController
    {
        public ProductsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<ProductDto>> BrowseProducts([FromQuery] BrowseProducts query) => await QueryAsync(query);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await QueryAsync(new GetProduct(id));
            if (product is null)
                return NotFound();

            return product;
        }
    }
}