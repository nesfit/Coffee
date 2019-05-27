using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.AuthenticationMeans;
using Barista.Api.Dto;
using Barista.Api.Models.Identity;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/authenticationMeans")]
    public class AuthenticationMeansController : BaseController
    {
        private readonly IIdentityService _identityService;

        public AuthenticationMeansController(IBusPublisher busPublisher, IIdentityService identityService) : base(busPublisher)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpGet]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<IPagedResult<AuthenticationMeans>>> BrowseAuthenticationMeans([FromQuery] BrowseAuthenticationMeans query)
            => Collection(await _identityService.BrowseAuthenticationMeans(query));

        [HttpGet("{id}")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(200, Type = typeof(AuthenticationMeans))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AuthenticationMeans>> GetAuthenticationMeans(Guid id)
            => Single(await _identityService.GetAuthenticationMeans(id));

        [HttpPost]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateAuthenticationMeans(AuthenticationMeansCreationDto dto)
            => await SendAndHandleIdentifierResultCommand(
                new CreateAuthenticationMeans(Guid.NewGuid(), dto.Label, dto.Type, dto.Value, dto.ValidSince, dto.ValidUntil),
                nameof(GetAuthenticationMeans)
            );

        [HttpPut("{id}")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateAuthenticationMeans(Guid id, AuthenticationMeansDto meansDto)
            => await SendAndHandleOperationCommand(new UpdateAuthenticationMeans(id, meansDto.Label, meansDto.Type, meansDto.ValidSince, meansDto.ValidUntil));

        [HttpPut("{id}/value")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult> UpdateAuthenticationMeansValue(Guid id, [FromBody] string valueToHash,
            [FromServices] IPasswordHasher<AuthenticationMeans> hasher)
        {
            var authenticationMeans = await _identityService.GetAuthenticationMeans(id);
            if (authenticationMeans is null)
                return NotFound();

            return await SendAndHandleOperationCommand(new UpdateAuthenticationMeansValue(id, hasher.HashPassword(authenticationMeans, valueToHash)));
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAuthenticationMeans(Guid id)
            => await SendAndHandleOperationCommand(new DeleteAuthenticationMeans(id));
    }
}
