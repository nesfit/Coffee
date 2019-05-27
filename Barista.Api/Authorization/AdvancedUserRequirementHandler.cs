using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Api.Models.Users;
using Barista.Api.ResourceAuthorization;
using Barista.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Barista.Api.Authorization
{
    public class AdvancedUserRequirementHandler : AuthorizationHandler<AdvancedUserRequirement>
    {
        private readonly IAccountingGroupsService _accountingGroupsService;
        private readonly IPointsOfSaleService _pointsOfSaleService;

        public AdvancedUserRequirementHandler(IAccountingGroupsService accountingGroupsService, IPointsOfSaleService pointsOfSaleService)
        {
            _accountingGroupsService = accountingGroupsService ?? throw new ArgumentNullException(nameof(accountingGroupsService));
            _pointsOfSaleService = pointsOfSaleService ?? throw new ArgumentNullException(nameof(pointsOfSaleService));
        }

        private static bool IsSuccessfulResponse(Task<HttpResponseMessage> antecedentTask)
        {
            return antecedentTask.IsCompleted && antecedentTask.Result?.StatusCode == HttpStatusCode.OK;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdvancedUserRequirement requirement)
        {
            if (context.User.HasAdministratorClaim())
                context.Succeed(requirement); // TODO log elevation

            var userId = context.User.GetUserId();

            // todo: check result after either task finishes
            var tasks = new[]
            {
                Task.Run(() => _accountingGroupsService.FindAuthorizedUser(userId)),
                Task.Run(() => _pointsOfSaleService.FindAuthorizedUser(userId))
            };

            await Task.WhenAll(tasks);

            if (tasks.Any(IsSuccessfulResponse))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
