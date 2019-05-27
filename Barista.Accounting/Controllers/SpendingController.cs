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
    [Route("api/spending")]
    [ApiController]
    public class SpendingController : BaristaController
    {
        public SpendingController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet("ofMeans/{meansId}")]
        public async Task<ActionResult<decimal>> GetSpendingOfAuthenticationMeans(Guid meansId)
            => await Dispatcher.QueryAsync(new GetSpendingOfMeans(meansId, null));

        [HttpGet("ofMeans/{meansId}/since/{since}")]
        public async Task<ActionResult<decimal>> GetSpendingOfAuthenticationMeans(Guid meansId, DateTimeOffset since)
            => await Dispatcher.QueryAsync(new GetSpendingOfMeans(meansId, since));

        [HttpGet("ofUser/{userId}")]
        public async Task<IPagedResult<SpendingOfUserDto>> GetSpendingOfUser(Guid userId, [FromQuery] BrowseSpendingOfUser query)
            => await Dispatcher.QueryAsync(query.Bind(q => q.UserId, userId));

        [HttpGet("ofUser/{userId}/sum")]
        public async Task<ActionResult<decimal>> GetSpendingOfUser(Guid userId, [FromQuery] GetSpendingOfUser query)
            => await Dispatcher.QueryAsync(query.Bind(q => q.UserId, userId));
    }
}
