using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.StockItem;
using Barista.Api.Dto;
using Barista.Api.Models.StockItems;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/stockItems")]
    public class StockItemsController : BaseController
    {
        private readonly IStockItemsService _stockItemsService;
        private readonly IPointOfSaleAuthorizationLoader _posAuthLoader;

        public StockItemsController(IBusPublisher busPublisher, IStockItemsService stockItemsService, IPointOfSaleAuthorizationLoader posAuthLoader) : base(busPublisher)
        {
            _stockItemsService = stockItemsService ?? throw new ArgumentNullException(nameof(stockItemsService));
            _posAuthLoader = posAuthLoader ?? throw new ArgumentNullException(nameof(posAuthLoader));
        }

        [HttpGet]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<IPagedResult<StockItem>>> BrowseStockItems([FromQuery] BrowseStockItems query)
            => Collection(await _stockItemsService.BrowseStockItems(query));

        [HttpGet("atPointOfSale/{posId}")]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult<IPagedResult<StockItem>>> BrowseStockItemsAtPointOfSale(Guid posId, [FromQuery] BrowseStockItems query)
        {
            if (query.AtPointOfSaleId != null && query.AtPointOfSaleId != posId)
                return BadRequest($"The '{nameof(query.AtPointOfSaleId)}' query option must equal the ID from URL or must be left unset");

            await _posAuthLoader.AssertResourceAccessAsync(User, posId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _stockItemsService.BrowseStockItems(query.Bind(q => q.AtPointOfSaleId, posId)));
        }

        [HttpGet("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(StockItem))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StockItem>> GetStockItem(Guid id)
        {
            var stockItem = await _stockItemsService.GetStockItem(id);
            if (stockItem is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, stockItem.PointOfSaleId, IsAuthorizedUserPolicy.Instance);
            return stockItem;
        }

        [HttpPost]
        [Authorize(Policies.IsAdvancedUser)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> CreateStockItem(StockItemDto stockItemDto)
        {
            await _posAuthLoader.AssertResourceAccessAsync(User, stockItemDto.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleIdentifierResultCommand(
                new CreateStockItem(Guid.NewGuid(), stockItemDto.DisplayName, stockItemDto.PointOfSaleId),
                nameof(GetStockItem)
            );
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsAdvancedUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateStockItem(Guid id, StockItemDto stockItemDto)
        {
            var stockItem = await _stockItemsService.GetStockItem(id);
            if (stockItem is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, stockItem.PointOfSaleId, IsAuthorizedUserPolicy.Instance);
            if (stockItem.PointOfSaleId != stockItemDto.PointOfSaleId)
                await _posAuthLoader.AssertResourceAccessAsync(User, stockItemDto.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(new UpdateStockItem(id, stockItemDto.DisplayName, stockItemDto.PointOfSaleId));
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsAdvancedUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteStockItem(Guid id)
        {
            var stockItem = await _stockItemsService.GetStockItem(id);
            if (stockItem is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, stockItem.PointOfSaleId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteStockItem(id));
        }
    }
}
