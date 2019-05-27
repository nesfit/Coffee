using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Barista.Api.Authenticators;
using Barista.Api.Authenticators.Impl;
using Barista.Api.Authorization;
using Barista.Api.Commands.AssignmentToPointOfSale;
using Barista.Api.Commands.AuthenticationMeans;
using Barista.Api.Commands.Operations;
using Barista.Api.Commands.PointOfSale;
using Barista.Api.Commands.PointOfSaleIntegration;
using Barista.Api.Dto;
using Barista.Api.Models;
using Barista.Api.Models.Identity;
using Barista.Api.Models.PointsOfSale;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Common.OperationResults;
using Barista.Contracts;
using Barista.Contracts.Commands.AssignmentToPointOfSale;
using Barista.Contracts.Commands.AuthenticationMeans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/pointsOfSale")]
    [ApiController]
    public class PointsOfSaleController : BaseController
    {
        private readonly IPointsOfSaleService _pointsOfSaleService;
        private readonly IPointOfSaleAuthorizationLoader _authLoader;
        private readonly IAccountingGroupAuthorizationLoader _accGroupAuthLoader;
        private readonly IIdentityService _idService;

        public PointsOfSaleController(IBusPublisher busPublisher, IPointsOfSaleService pointsOfSaleService, IPointOfSaleAuthorizationLoader authLoader, IAccountingGroupAuthorizationLoader accGroupAuthLoader, IIdentityService idService) : base(busPublisher)
        {
            _pointsOfSaleService = pointsOfSaleService ?? throw new ArgumentNullException(nameof(pointsOfSaleService));
            _authLoader = authLoader ?? throw new ArgumentNullException(nameof(authLoader));
            _accGroupAuthLoader = accGroupAuthLoader ?? throw new ArgumentNullException(nameof(accGroupAuthLoader));
            _idService = idService ?? throw new ArgumentNullException(nameof(idService));
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResult<PointOfSale>>> BrowsePointsOfSale([FromQuery] BrowsePointsOfSale query) =>
            Collection(await _pointsOfSaleService.BrowsePointsOfSale(query));

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PointOfSale>> GetPointOfSale(Guid id) =>
            Single(await _pointsOfSaleService.GetPointOfSale(id));

        [HttpGet("{id}/authorizedUsers")]
        [ProducesResponseType(200, Type = typeof(IPagedResult<PointOfSaleAuthorizedUser>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IPagedResult<PointOfSaleAuthorizedUser>>> BrowseAuthorizedUsers(Guid id, [FromQuery] BrowsePointOfSaleAuthorizedUsers query) =>
            Collection(await _pointsOfSaleService.BrowseAuthorizedUsers(id, query));

        [HttpGet("{id}/authorizedUsers/{userId}")]
        [ProducesResponseType(200, Type = typeof(PointOfSaleAuthorizedUser))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PointOfSaleAuthorizedUser>> GetAuthorizedUser(Guid id, Guid userId) =>
            Single(await _pointsOfSaleService.GetAuthorizedUser(id, userId));

        [HttpPost("{id}/authorizedUsers/{userId}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CreateAuthorizedUser(Guid id, Guid userId, UserAuthorizationDto userAuthorization)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new CreatePointOfSaleUserAuthorization(id, userId, userAuthorization.Level));
        }

        [HttpDelete("{id}/authorizedUsers/{userId}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAuthorizedUser(Guid id, Guid userId)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeletePointOfSaleUserAuthorization(id, userId));
        }

        [HttpPut("{id}/authorizedUsers/{userId}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateAuthorizedUser(Guid id, Guid userId, UserAuthorizationDto userAuthorization)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new UpdatePointOfSaleUserAuthorization(id, userId, userAuthorization.Level));
        }   

        [HttpPost]
        [Authorize(Policies.CreatePointsOfSale)]
        [ProducesResponseType(202)]
        public async Task<ActionResult> CreatePointOfSale(HandleCreationOfPointOfSale command)
        {
            await _accGroupAuthLoader.AssertResourceAccessAsync(User, command.ParentAccountingGroupId, IsAuthorizedUserPolicy.Instance);

            var posId = Guid.NewGuid();
            return await StartLongRunningOperation(
                command
                    .Bind(c => c.Id, posId)
                    .Bind(c => c.OwnerUserId, User.GetUserId()),

                posId
            );
        }

        [HttpPut("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdatePointOfSale(Guid id, PointOfSaleDto posDto)
        {
            var pos = await _pointsOfSaleService.GetPointOfSale(id);
            if (pos is null)
                return new UnauthorizedObjectResult(new ErrorDto("point_of_sale_not_found", $"Could not find point of sale with ID '{id}'"));

            await _authLoader.AssertResourceAccessAsync(User, id, IsAuthorizedUserPolicy.Instance);

            if (pos.ParentAccountingGroupId != posDto.ParentAccountingGroupId)
                await _accGroupAuthLoader.AssertResourceAccessAsync(User, posDto.ParentAccountingGroupId, IsAuthorizedUserPolicy.Instance);
            
            return await SendAndHandleOperationCommand(new UpdatePointOfSale(id, posDto.DisplayName, posDto.ParentAccountingGroupId, posDto.SaleStrategyId, posDto.Features));
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeletePointOfSale(Guid id)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeletePointOfSale(id));
        }

        [HttpPut("{id}/keyValues/{key}")]
        [Authorize(Policies.IsPointOfSale)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> SetPointOfSaleKeyValue(Guid id, string key, [FromBody] string value)
        {
            var command = new SetPointOfSaleKeyValue(id, key, value);

            if (User.IsPointOfSale())
                command = command.Bind(cmd => cmd.PointOfSaleId, User.GetPointOfSaleId());

            return await SendAndHandleOperationCommand(command);
        }

        [HttpPost("{id}/command/powerOn")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SendPowerOnCommand(Guid id)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new PowerOn(id));
        }

        [HttpPost("{id}/command/powerOff")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SendPowerOffCommand(Guid id)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new PowerOff(id));
        }

        [HttpPost("{id}/command/startCleaning")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SendStartCleaningCommand(Guid id)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new StartCleaning(id));
        }

        [HttpPost("{id}/command/dispenseProduct")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> DispenseProduct(Guid id, DispenseProductDto dispenseProductDto)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            return await SendAndHandleOperationCommand(new DispenseProduct(id, dispenseProductDto.ProductId));
        }

        [HttpGet("{id}/apiKeys")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IPagedResult<AuthenticationMeans>>> BrowseApiKeys(Guid id, [FromQuery] BrowseAssignedMeans query)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            query.Method = MeansMethod.ApiKey;
            return Collection(await _idService.BrowseMeansAssignedToPointOfSaleNoValue(id, query));
        }

        [HttpPost("{id}/apiKeys")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<CreatedApiKey>> CreateApiKey(Guid id, ApiKeyDto dto, [FromServices] IApiKeyGenerator apiKeyGenerator, [FromServices] IMeansValueHasher hasher)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);
            var apiKey = apiKeyGenerator.Generate();

            var meansResult = await Publisher.SendRequest<ICreateAuthenticationMeans, IIdentifierResult>(
                new CreateAuthenticationMeans(
                    Guid.NewGuid(), dto.Label, MeansMethod.ApiKey, hasher.Hash(apiKey), DateTimeOffset.UtcNow, null
                ));

            if (!meansResult.Successful || !meansResult.Id.HasValue)
                throw meansResult.ToException();

            await Publisher.SendRequest<ICreateAssignmentToPointOfSale, IIdentifierResult>(
                new CreateAssignmentToPointOfSale(Guid.NewGuid(), meansResult.Id.Value, DateTimeOffset.UtcNow, null, id)
            );

            return new CreatedApiKey(meansResult.Id.Value, apiKey);
        }

        [HttpDelete("{id}/apiKeys/{meansId}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteApiKey(Guid id, Guid meansId)
        {
            await _authLoader.AssertResourceAccessAsync(User, id, IsOwnerPolicy.Instance);

            var resultsPage = await _idService.BrowseAssignmentsToPointOfSale(new BrowseAssignmentsToPointOfSale {AssignedToPointOfSale = id, OfAuthenticationMeans = meansId});
            if (resultsPage.Items.Count != 1)
                return NotFound();

            return await SendAndHandleOperationCommand(new DeleteAuthenticationMeans(meansId));
        }
    }
}