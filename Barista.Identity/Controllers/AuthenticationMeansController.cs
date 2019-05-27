using System;
using System.Threading.Tasks;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Identity.Controllers
{
    [Route("api/authenticationMeans")]
    [ApiController]
    public class AuthenticationMeansController : BaristaController
    {
        public AuthenticationMeansController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<AuthenticationMeansDto>> BrowseAuthenticationMeans([FromQuery] BrowseAuthenticationMeans command)
            => await QueryAsync(command);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<AuthenticationMeansDto>> GetAuthenticationMeans(Guid id)
        {
            var authenticationMeans = await QueryAsync(new GetAuthenticationMeans(id));
            if (authenticationMeans is null)
                return NotFound();

            return authenticationMeans;
        }
    }
}
