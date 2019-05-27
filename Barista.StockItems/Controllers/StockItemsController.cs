using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.StockItems.Dto;
using Barista.StockItems.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.StockItems.Controllers
{
    [Route("api/stockItems")]
    [ApiController]
    public class StockItemsController : BaristaController
    {
        public StockItemsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<StockItemDto>> BrowseStockItems([FromQuery] BrowseStockItems query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<StockItemDto>> GetStockItem(Guid id)
        {
            var stockItem = await QueryAsync(new GetStockItem(id));
            if (stockItem is null)
                return NotFound();

            return stockItem;
        }
    }
}