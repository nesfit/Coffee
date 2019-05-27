using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Dto;
using Barista.Api.Models.Accounting;
using Barista.Api.Queries;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [Route("api/spending")]
    [ApiController]
    public class SpendingController : BaseController
    {
        private readonly IAccountingService _accountingService;
        private readonly IAuthorizationService _authService;

        public SpendingController(IBusPublisher busPublisher, IAccountingService accountingService, IAuthorizationService authService) : base(busPublisher)
        {
            _accountingService = accountingService ?? throw new ArgumentNullException(nameof(accountingService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [Authorize(Policies.IsUser)]
        [HttpGet("ofUser/{userId}")]
        public async Task<ActionResult<IPagedResult<SpendingOfUser>>> BrowseSpendingOfUser(Guid userId, [FromQuery] BrowseSpendingOfUser query)
        {
            if (User.GetUserId() == userId || await _authService.IsAdministrator(User))
                return Collection(await _accountingService.BrowseSpendingOfUser(userId, query));
            else
                return Unauthorized(new ErrorDto("unauthorized_user_id", "Only administrators can access other users' spending."));
        }

        [Authorize(Policies.IsUser)]
        [HttpGet("ofUser/{userId}/sum")]
        public async Task<ActionResult<SpendingSumDto>> SumSpendingOfUser(Guid userId, [FromQuery] BrowseSpendingOfUser query)
        {
            if (User.GetUserId() == userId || await _authService.IsAdministrator(User))
                return new SpendingSumDto(await _accountingService.SumSpendingOfUser(userId, query));
            else
                return Unauthorized(new ErrorDto("unauthorized_user_id", "Only administrators can access other users' spending."));
        }

        [Authorize(Policies.IsUser)]
        [HttpGet("ofUser/me")]
        public async Task<ActionResult<IPagedResult<SpendingOfUser>>> BrowseSpendingOfCurrentUser([FromQuery] BrowseSpendingOfUser query)
            => await BrowseSpendingOfUser(User.GetUserId(), query);

        [Authorize(Policies.IsUser)]
        [HttpGet("ofUser/me/sum")]
        public async Task<ActionResult<SpendingSumDto>> SumSpendingOfCurrentUser([FromQuery] BrowseSpendingOfUser query)
            => await SumSpendingOfUser(User.GetUserId(), query);
    }
}
