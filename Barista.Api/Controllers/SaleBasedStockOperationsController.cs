using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.SaleBasedStockOperation;
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
    [Route("api/saleBasedStockOperations")]
    public class SaleBasedStockOperationsController : BaseController
    {
        private readonly IStockOperationsService _stockOperationsService;
        private readonly IStockItemAuthorizationLoader _stockItemAuthorizationLoader;
        private readonly IAuthorizationService _authService;

        public SaleBasedStockOperationsController(IBusPublisher busPublisher, IStockOperationsService stockOperationsService, IStockItemAuthorizationLoader stockItemAuthorizationLoader, IAuthorizationService authService) : base(busPublisher)
        {
            _stockOperationsService = stockOperationsService ?? throw new ArgumentNullException(nameof(stockOperationsService));
            _stockItemAuthorizationLoader = stockItemAuthorizationLoader ?? throw new ArgumentNullException(nameof(stockItemAuthorizationLoader));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(PagedResult<SaleBasedStockOperation>))]
        public async Task<ActionResult<IPagedResult<SaleBasedStockOperation>>> BrowseSaleBasedStockOperations([FromQuery] BrowseStockOperations query)
        {
            if ((query.StockItemId is null || query.StockItemId.Length == 0) && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new ErrorDto("unauthorized_resource_access", "Only administrators may view all sale-based stock operations."));

            await _stockItemAuthorizationLoader.AssertMultipleResourceAccessAsync(User, query.StockItemId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _stockOperationsService.BrowseSaleBasedStockOperations(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(SaleBasedStockOperation))]
        public async Task<ActionResult<SaleBasedStockOperation>> GetSaleBasedStockOperation(Guid id)
        {
            var stockOperation = await _stockOperationsService.GetSaleBasedStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            return stockOperation;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteSaleBasedStockOperation(Guid id)
        {
            var stockOperation = await _stockOperationsService.GetSaleBasedStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteSaleBasedStockOperation(id));
        }
    }
}
