using System;
using System.Threading.Tasks;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Accounting.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : BaristaController
    {
        public SalesController(IDispatcher dispatcher) : base(dispatcher)
        {
        }
        
        [HttpGet]
        public async Task<IPagedResult<SaleDto>> BrowseSales([FromQuery] BrowseSales query)
        {
            return await QueryAsync<IPagedResult<SaleDto>>(query);
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(Guid id)
        {
            var sale = await QueryAsync(new GetSale(id));
            if (sale is null)
                return NotFound();

            return sale;
        }

        [HttpGet("{parentSaleId}/stateChanges")]
        public async Task<IPagedResult<SaleStateChangeDto>> BrowseSaleStateChanges(Guid parentSaleId, [FromQuery] BrowseSaleStateChanges query)
        {
            query.Bind(q => q.ParentSaleId, parentSaleId);
            return await QueryAsync(query);
        }

        [HttpGet("{parentSaleId}/stateChanges/{saleStateChangeId}")]
        public async Task<ActionResult<SaleStateChangeDto>> GetSaleStateChange(Guid parentSaleId, Guid saleStateChangeId)
        {
            var chg = await QueryAsync(new GetSaleStateChange(saleStateChangeId, parentSaleId));
            if (chg is null)
                return NotFound();

            return chg;
        }
    }
}