using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Models.StockOperations;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/stockOperations")]
    public class StockOperationsController : BaseController
    {
        private readonly IStockOperationsService _stockOperationsService;
        private readonly IStockItemAuthorizationLoader _stockItemAuthorizationLoader;
        private readonly IAuthorizationService _authService;

        public StockOperationsController(IBusPublisher busPublisher, IStockOperationsService stockOperationsService, IStockItemAuthorizationLoader stockItemAuthorizationLoader, IAuthorizationService authService) : base(busPublisher)
        {
            _stockOperationsService = stockOperationsService ?? throw new ArgumentNullException(nameof(stockOperationsService));
            _stockItemAuthorizationLoader = stockItemAuthorizationLoader ?? throw new ArgumentNullException(nameof(stockItemAuthorizationLoader));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(ResultPage<StockOperation>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<StockOperation>>> BrowseStockOperations([FromQuery] BrowseStockOperations query)
        {
            if ((query.StockItemId is null || query.StockItemId.Length == 0) && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new ErrorDto("unauthorized_resource_access", "Only administrators may view all stock operations."));

            await _stockItemAuthorizationLoader.AssertMultipleResourceAccessAsync(User, query.StockItemId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _stockOperationsService.BrowseStockOperations(query));
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(StockOperation))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StockOperation>> GetStockOperation(Guid id)
        {
            var stockOperation = await _stockOperationsService.GetStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            return stockOperation;
        }

        [HttpGet("balance/{stockItemId}")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<decimal>> GetStockItemBalance(Guid stockItemId)
        {
            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockItemId, IsAuthorizedUserPolicy.Instance);
            return await _stockOperationsService.GetStockItemBalance(stockItemId);
        }
    }
}
