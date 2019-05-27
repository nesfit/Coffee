using System;
using System.Threading.Tasks;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Accounting.Controllers
{
    [Route("api/balance")]
    [ApiController]
    public class BalanceController : BaristaController
    {
        public BalanceController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<BalanceDto>> GetUserBalance(Guid userId) =>
            await QueryAsync(new GetBalance(userId));
    }
}
