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
    [Route("api/saleBasedStockOperations")]
    [ApiController]
    public class SaleBasedStockOperationsController : BaristaController
    {
        public SaleBasedStockOperationsController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<SaleBasedStockOperationDto>> BrowseSaleBasedStockOperations([FromQuery] BrowseSaleBasedStockOperations query)
        {
            return await QueryAsync(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<SaleBasedStockOperationDto>> GetSaleBasedStockOperation(Guid id)
        {
            var operation = await QueryAsync(new GetSaleBasedStockOperation(id));
            if (operation is null)
                return NotFound();

            return operation;
        }
    }
}