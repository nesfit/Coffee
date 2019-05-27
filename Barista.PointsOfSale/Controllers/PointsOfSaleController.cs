using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.PointsOfSale.Controllers
{
    [Route("api/pointsOfSale")]
    [ApiController]
    public class PointsOfSaleController : BaristaController
    {
        public PointsOfSaleController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<PointOfSaleDto>> BrowsePointsOfSale([FromQuery] BrowsePointsOfSale query) => await QueryAsync(query);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<PointOfSaleDto>> GetPointOfSale(Guid id)
        {
            var pos = await QueryAsync(new GetPointOfSale(id));
            if (pos is null)
                return NotFound();

            return pos;
        }

        [HttpGet("{pointOfSaleId}/authorizedUsers")]
        public async Task<IPagedResult<UserAuthorizationDto>> BrowseAuthorizedUsers(Guid pointOfSaleId, [FromQuery] BrowseUserAuthorizations query)
        {
            query.Bind(q => q.PointOfSaleId, pointOfSaleId);
            return await QueryAsync(query);
        }

        [HttpGet("{pointOfSaleId}/authorizedUsers/{userId}")]
        public async Task<ActionResult<UserAuthorizationDto>> GetUserAuthorization(Guid pointOfSaleId, Guid userId)
        {
            var result = await QueryAsync(new GetUserAuthorization(pointOfSaleId, userId));
            if (result is null)
                return NotFound();

            return result;
        }

        [HttpGet("userAuthorizations/{userId}")]
        public async Task<IPagedResult<UserAuthorizationDto>> BrowseUserAuthorizations([FromQuery] BrowseUserAuthorizations query, Guid userId)
            => await QueryAsync(query.Bind(q => q.UserId, userId));

        [HttpHead("authorizedUsers/{userId}")]
        public async Task<IActionResult> FindAuthorizedUser(Guid userId)
        {
            var result = await QueryAsync(new BrowseUserAuthorizations() { UserId = userId, ResultsPerPage = 1 });
            return result.TotalResults > 0 ? (IActionResult)Ok() : NoContent();
        }
    }
}
