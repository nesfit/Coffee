using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.ManualStockOperation;
using Barista.Api.Dto;
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
    [Route("api/manualStockOperations")]
    public class ManualStockOperationsController : BaseController
    {
        private readonly IStockOperationsService _stockOperationsService;
        private readonly IStockItemAuthorizationLoader _stockItemAuthorizationLoader;
        private readonly IAuthorizationService _authService;

        public ManualStockOperationsController(IBusPublisher busPublisher, IStockOperationsService stockOperationsService, IStockItemAuthorizationLoader stockItemAuthorizationLoader, IAuthorizationService authService) : base(busPublisher)
        {
            _stockOperationsService = stockOperationsService ?? throw new ArgumentNullException(nameof(stockOperationsService));
            _stockItemAuthorizationLoader = stockItemAuthorizationLoader ?? throw new ArgumentNullException(nameof(stockItemAuthorizationLoader));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(PagedResult<ManualStockOperation>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<ManualStockOperation>>> BrowseManualStockOperations([FromQuery] BrowseStockOperations query)
        {
            if ((query.StockItemId is null || query.StockItemId.Length == 0) && !(await _authService.IsAdministrator(User)))
                return Unauthorized(new ErrorDto("unauthorized_resource_access", "Only administrators may view all manual stock operations."));

            await _stockItemAuthorizationLoader.AssertMultipleResourceAccessAsync(User, query.StockItemId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _stockOperationsService.BrowseManualStockOperations(query));
        }

        [HttpGet("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(ManualStockOperation))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ManualStockOperation>> GetManualStockOperation(Guid id)
        {
            var stockOperation = await _stockOperationsService.GetManualStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            return stockOperation;
        }

        [HttpPost]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> CreateManualStockOperation(ManualStockOperationDto manualStockOperationDto)
        {
            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, manualStockOperationDto.StockItemId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleIdentifierResultCommand(
                new CreateManualStockOperation(Guid.NewGuid(), manualStockOperationDto.StockItemId, manualStockOperationDto.Quantity, User.GetUserId(), manualStockOperationDto.Comment),
                nameof(GetManualStockOperation)
            );
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> UpdateManualStockOperation(Guid id, ManualStockOperationDto manualStockOperationDto)
        {
            var stockOperation = await _stockOperationsService.GetStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            if (manualStockOperationDto.StockItemId != stockOperation.StockItemId)
                await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, manualStockOperationDto.StockItemId, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new UpdateManualStockOperation(id, manualStockOperationDto.StockItemId, manualStockOperationDto.Quantity, User.GetUserId(), manualStockOperationDto.Comment)
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteManualStockOperation(Guid id)
        {
            var stockOperation = await _stockOperationsService.GetStockOperation(id);
            if (stockOperation is null)
                return NotFound();

            await _stockItemAuthorizationLoader.AssertResourceAccessAsync(User, stockOperation.StockItemId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteManualStockOperation(id));
        }
    }
}
