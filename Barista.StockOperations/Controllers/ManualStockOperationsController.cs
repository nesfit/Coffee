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
    [Route("api/manualStockOperations")]
    [ApiController]
    public class ManualStockOperationsController : BaristaController
    {
        public ManualStockOperationsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<ManualStockOperationDto>> BrowseManualStockOperations([FromQuery] BrowseManualStockOperations query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<ManualStockOperationDto>> GetManualStockOperation(Guid id)
        {
            var operation = await QueryAsync(new GetManualStockOperation(id));
            if (operation is null)
                return NotFound();

            return operation;
        }
    }
}