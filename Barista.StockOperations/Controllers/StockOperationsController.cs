using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.StockOperations.Dto;
using Barista.StockOperations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.StockOperations.Controllers
{
    [Route("api/stockOperations")]
    [ApiController]
    public class StockOperationsController : BaristaController
    {
        public StockOperationsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<StockOperationDto>> BrowseStockOperations([FromQuery] BrowseStockOperations query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<StockOperationDto>> GetStockOperation(Guid id)
        {
            var operation = await QueryAsync(new GetStockOperation(id));
            if (operation is null)
                return NotFound();

            return operation;
        }

        [HttpGet("balance/{stockItemId}")]
        public async Task<ActionResult<decimal>> GetStockItemBalance(Guid stockItemId)
        {
            return await QueryAsync(new GetStockItemBalance {StockItemId = stockItemId});
        }
    }
}