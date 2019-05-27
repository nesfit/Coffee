using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Api.Authenticators;
using Barista.Api.Authenticators.Impl;
using Barista.Api.Authorization;
using Barista.Api.Commands.AssignmentToPointOfSale;
using Barista.Api.Commands.AssignmentToUser;
using Barista.Api.Commands.AuthenticationMeans;
using Barista.Api.Commands.User;
using Barista.Api.Dto;
using Barista.Api.Models;
using Barista.Api.Models.Identity;
using Barista.Api.Models.Users;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Common.OperationResults;
using Barista.Contracts;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Contracts.Commands.AssignmentToUser;
using Barista.Contracts.Commands.AuthenticationMeans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UsersController(IBusPublisher busPublisher, IUsersService usersService, IMapper mapper, IIdentityService identityService, ILogger<UsersController> logger) : base(busPublisher)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize(Policies.BrowseUserSummaries)]
        [ProducesResponseType(200, Type = typeof(IPagedResult<User>))]
        public async Task<ActionResult<IPagedResult<User>>> BrowseUserSummaries([FromQuery] BrowseUsers query)
        {
            var pagedResult = await _usersService.BrowseUsers(query);
            return Collection(_mapper.Map<IPagedResult<User>>(pagedResult));
        }

        [HttpGet("details")]
        [Authorize(Policies.BrowseUserDetails)]
        [ProducesResponseType(200, Type = typeof(IPagedResult<FullUser>))]
        public async Task<ActionResult<IPagedResult<FullUser>>> BrowseUserDetails([FromQuery] BrowseDetailedUsers query)
        {
            var pagedResult = await _usersService.BrowseDetailedUsers(query);
            return Collection(pagedResult);
        }

        [HttpGet("{id}")]
        [Authorize(Policies.ViewUserSummary)]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUserSummary(Guid id)
            => Single(_mapper.Map<User>(await _usersService.GetUser(id)));

        [HttpGet("{id}/details")]
        [Authorize(Policies.ViewUserDetails)]
        [ProducesResponseType(200, Type = typeof(FullUser))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<FullUser>> GetUserDetails(Guid id)
            => Single(await _usersService.GetUser(id));

        [HttpGet("me")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(FullUser))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<FullUser>> GetCurrentUser()
            => Single(await _usersService.GetUser(User.GetUserId()));

        [Authorize(Policies.CreateUsers)]
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserDto userDto)
        {
            var command = new CreateUser(Guid.NewGuid(), userDto.FullName, userDto.EmailAddress, userDto.IsAdministrator, userDto.IsActive);
            return await SendAndHandleIdentifierResultCommand(command, nameof(GetUserSummary));
        }

        [Authorize(Policies.UpdateUsers)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, UserDto userDto)
        {
            var command = new UpdateUser(id, userDto.FullName, userDto.EmailAddress, userDto.IsAdministrator, userDto.IsActive);
            return await SendAndHandleOperationCommand(command);
        }

        [Authorize(Policies.IsUser)]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateCurrentUser(CurrentUserDto userDto)
        {
            var userId = User.GetUserId();
            var user = await _usersService.GetUser(userId);
            if (user is null)
                return NotFound();

            var command = new UpdateUser(userId, userDto.FullName, userDto.EmailAddress, user.IsAdministrator, user.IsActive);
            return await SendAndHandleOperationCommand(command);
        }

        [Authorize(Policies.IsUser)]
        [HttpPost("me/changePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto newPwDto, [FromServices] IUserPasswordAuthenticator userPwAuthenticator, [FromServices] IMeansValueHasher pwHasher)
        {
            var userId = User.GetUserId();
            var user = await _usersService.GetUser(userId);
            if (user is null)
                return NotFound();

            var authResult = await userPwAuthenticator.AuthenticateAsync(user.EmailAddress, newPwDto.OldPassword);
            if (authResult is null)
                return BadRequest(new ErrorDto("invalid_old_password", "The old password does not match. Please try again."));
           
            if (newPwDto.NewPassword != newPwDto.NewPasswordAgain)
                return BadRequest(new ErrorDto("invalid_new_passwords", "The new passwords do not match. Please try again."));

            return await SendAndHandleOperationCommand(new UpdateAuthenticationMeansValue(authResult.MeansId, pwHasher.Hash(newPwDto.NewPassword)));
        }

        [Authorize(Policies.IsAdministrator)]
        [HttpPut("{id}/password")]
        public async Task<ActionResult> SetPassword(Guid id, SetPasswordDto pwDto, [FromServices] IMeansValueHasher pwHasher)
        {
            var userId = id;

            if ((await _usersService.GetUser(userId)) is null)
                throw new BaristaException("user_not_found", $"Could not find user with ID '{userId}'");

            var passwordPage = await _identityService.BrowseMeansAssignedToUser(userId, new BrowseAssignedMeans { IsValid = true, Method = MeansMethod.Password, ResultsPerPage = 1});

            if (passwordPage.TotalResults == 1)
            {
                var means = passwordPage.Items.Single();
                return await SendAndHandleOperationCommand(new UpdateAuthenticationMeansValue(means.Id, pwHasher.Hash(pwDto.NewPassword)));
            }
            else if (passwordPage.TotalResults > 1)
            {
                _logger.LogWarning("The user {userId} has more than one password authentication means assigned. They will be purged before the new password is set.", userId);

                int deletedPasswordMeans;
                var query = new BrowseAssignedMeans {IsValid = true, Method = MeansMethod.Password};

                do
                {
                    deletedPasswordMeans = 0;

                    var passwordMeansPage = await _identityService.BrowseMeansAssignedToUser(userId, query);
                    foreach (var passwordMeans in passwordMeansPage.Items)
                    {
                        await Publisher.SendRequest(new DeleteAuthenticationMeans(passwordMeans.Id));
                        _logger.LogInformation("Purged one of multiple passwords assigned to user {userId} with means ID {meansId}", userId, passwordMeans.Id);
                        deletedPasswordMeans++;
                    }
                } while (deletedPasswordMeans > 0);
            }

            var meansCreation = await Publisher.SendRequest<ICreateAuthenticationMeans, IIdentifierResult>(
                new CreateAuthenticationMeans(Guid.NewGuid(), null, MeansMethod.Password, pwHasher.Hash(pwDto.NewPassword), DateTimeOffset.UtcNow, null)
            );

            if (!meansCreation.Successful)
                throw meansCreation.ToException();

            var meansAssignment = await Publisher.SendRequest<ICreateAssignmentToUser, IIdentifierResult>(
                new CreateAssignmentToUser(Guid.NewGuid(), meansCreation.Id.Value, DateTimeOffset.UtcNow, null, userId, false)
            );

            if (!meansAssignment.Successful)
                throw meansAssignment.ToException();

            return Ok();
        }

        [Authorize(Policies.DeleteUsers)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            return await SendAndHandleOperationCommand(new DeleteUser(id));
        }

        [HttpGet("{id}/apiKeys")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IPagedResult<AuthenticationMeans>>> BrowseApiKeys(Guid id, [FromQuery] BrowseAssignedMeans query)
        {
            query.Method = MeansMethod.ApiKey;
            return Collection(await _identityService.BrowseMeansAssignedToUserNoValue(id, query));
        }

        [HttpPost("{id}/apiKeys")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<CreatedApiKey>> CreateApiKey(Guid id, ApiKeyDto dto, [FromServices] IApiKeyGenerator apiKeyGenerator, [FromServices] IMeansValueHasher hasher)
        {
            var apiKey = apiKeyGenerator.Generate();

            var meansResult = await Publisher.SendRequest<ICreateAuthenticationMeans, IIdentifierResult>(
                new CreateAuthenticationMeans(
                    Guid.NewGuid(), dto.Label, MeansMethod.ApiKey, hasher.Hash(apiKey), DateTimeOffset.UtcNow, null
                ));

            if (!meansResult.Successful || !meansResult.Id.HasValue)
                throw meansResult.ToException();

            await Publisher.SendRequest<ICreateAssignmentToUser, IIdentifierResult>(
                new CreateAssignmentToUser(Guid.NewGuid(), meansResult.Id.Value, DateTimeOffset.UtcNow, null, id, false)
            );

            return new CreatedApiKey(meansResult.Id.Value, apiKey);
        }

        [HttpDelete("{id}/apiKeys/{meansId}")]
        [Authorize(Policies.IsAdministrator)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteApiKey(Guid id, Guid meansId)
        {
            var resultsPage = await _identityService.BrowseAssignmentsToUser(new BrowseAssignmentsToUser { AssignedToUser = id, OfAuthenticationMeans = meansId });
            if (resultsPage.Items.Count != 1)
                return NotFound();

            return await SendAndHandleOperationCommand(new DeleteAuthenticationMeans(meansId));
        }

        [HttpGet("{id}/assignedMeans")]
        [Authorize(Policies.IsAdministrator)]
        public async Task<ActionResult<IPagedResult<AuthenticationMeans>>> BrowseAssignedMeans(Guid id, [FromQuery] BrowseAssignedMeans query)
            => Collection(await _identityService.BrowseMeansAssignedToUserNoValue(id, query));
    }
}
