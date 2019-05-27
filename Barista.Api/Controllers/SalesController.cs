using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.SaleStateChange;
using Barista.Api.Dto;
using Barista.Api.Models.Accounting;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly IAccountingService _accountingService;
        private readonly IPointsOfSaleService _pointsOfSaleService;

        private readonly IPosAgAuthorizationLoader _posAgAuthorizationLoader;
        private readonly IAccountingGroupAuthorizationLoader _agAuthLoader;

        public SalesController(IBusPublisher busPublisher, IAccountingService accountingService, IPointsOfSaleService pointsOfSaleService, IPosAgAuthorizationLoader posAgAuthorizationLoader, IAccountingGroupAuthorizationLoader agAuthLoader) : base(busPublisher)
        {
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
            _pointsOfSaleService = pointsOfSaleService ?? throw new ArgumentNullException(nameof(pointsOfSaleService));
            _posAgAuthorizationLoader = posAgAuthorizationLoader;
            _agAuthLoader = agAuthLoader;
        }

        [Authorize(Policies.IsAdministrator)]
        [HttpGet]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<Sale>>> BrowseSales([FromQuery] BrowseSales query) =>
            Collection(await _accountingService.BrowseSales(query));

        [Authorize(Policies.IsUser)]
        [HttpGet("me")]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IPagedResult<Sale>>> BrowseOwnSales([FromQuery] BrowseSales query) =>
            Collection(await _accountingService.BrowseSales(query.Bind(q => q.MadeByUser, User.GetUserId())));

        [Authorize(Policies.IsUser)]
        [HttpGet("pointOfSale/{posId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IPagedResult<Sale>>> BrowseByPointOfSale(Guid posId, [FromQuery] BrowseSales query)
        {
            var pos = await _pointsOfSaleService.GetPointOfSale(posId);
            if (pos is null)
                return NotFound();

            await _posAgAuthorizationLoader.AssertResourceAccessAsync(User, (posId, pos.ParentAccountingGroupId), IsAuthorizedUserPolicy.Instance);

            return Collection(await _accountingService.BrowseSales(query.Bind(q => q.MadeByPointOfSale, posId)));
        }

        [Authorize(Policies.IsUser)]
        [HttpGet("accountingGroup/{accountingGroupId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IPagedResult<Sale>>> BrowseByAccountingGroup(Guid accountingGroupId, [FromQuery] BrowseSales query)
        {
            await _agAuthLoader.AssertResourceAccessAsync(User, accountingGroupId, IsAuthorizedUserPolicy.Instance);
            return Collection(await _accountingService.BrowseSales(query.Bind(q => q.MadeInAccountingGroup, accountingGroupId)));
        }


        [HttpGet("{id}")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(200, Type = typeof(Sale))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Sale>> GetSale(Guid id)
        {
            var sale = await _accountingService.GetSale(id);
            if (sale is null)
                return NotFound();

            if (sale.UserId != User.GetUserId())
                await _posAgAuthorizationLoader.AssertResourceAccessAsync(User, (sale.PointOfSaleId, sale.AccountingGroupId), IsAuthorizedUserPolicy.Instance);

            return sale;
        }

        [HttpPost("{id}/cancel")]
        [Authorize(Policies.IsUser)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CancelSale(Guid id, CancelSaleByUser command)
        {
            var sale = await _accountingService.GetSale(id);
            if (sale is null)
                return NotFound();

            if (sale.UserId != User.GetUserId())
                await _posAgAuthorizationLoader.AssertResourceAccessAsync(User, (sale.PointOfSaleId, sale.AccountingGroupId), IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new CreateSaleStateChange(id, Guid.NewGuid(), command.Reason, "Cancelled", null, User.GetUserId())
            );
        }
    }
}