using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.AspNetCore;
using Barista.Common.Dispatchers;
using Barista.Contracts;
using Barista.Users.Dto;
using Barista.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaristaController
    {
        public UsersController(IDispatcher dispatcher) : base(dispatcher)
        {
        }

        [HttpGet]
        public async Task<IPagedResult<UserDto>> BrowseUsers([FromQuery] BrowseUsers query) => await QueryAsync(query);

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await QueryAsync(new GetUser(id));
            if (user is null)
                return NotFound();

            return user;
        }
    }
}