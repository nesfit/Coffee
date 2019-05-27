using System;
using System.Threading.Tasks;
using Barista.Api.Models.SaleStrategies;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/saleStrategies")]
    public class SaleStrategiesController : BaseController
    {
        private readonly ISaleStrategiesService _saleStrategiesService;

        public SaleStrategiesController(IBusPublisher busPublisher, ISaleStrategiesService saleStrategiesService) : base(busPublisher)
        {
            _saleStrategiesService = saleStrategiesService ?? throw new ArgumentNullException(nameof(saleStrategiesService));
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResult<SaleStrategy>>> BrowseSaleStrategies([FromQuery] DisplayNameQuery query)
            => Collection(await _saleStrategiesService.BrowseSaleStrategies(query));

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SaleStrategy))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SaleStrategy>> GetSaleStrategy(Guid id)
            => Single(await _saleStrategiesService.GetSaleStrategy(id));
    }
}
