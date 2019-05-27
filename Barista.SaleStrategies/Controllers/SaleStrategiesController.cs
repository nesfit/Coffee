using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.SaleStrategies.Dto;
using Barista.SaleStrategies.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.SaleStrategies.Controllers
{
    [Route("api/saleStrategies")]
    [ApiController]
    public class SaleStrategiesController : BaristaController
    {
        public SaleStrategiesController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<SaleStrategyDto>> BrowseSaleStrategies([FromQuery] BrowseSaleStrategies query) => await QueryAsync(query);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<SaleStrategyDto>> GetSaleStrategy(Guid id)
        {
            var saleStrategy = await QueryAsync(new GetSaleStrategy(id));
            if (saleStrategy is null)
                return NotFound();

            return saleStrategy;
        }
    }
}