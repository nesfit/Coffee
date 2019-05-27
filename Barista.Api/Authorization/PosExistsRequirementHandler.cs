using System;
using System.Threading.Tasks;
using Barista.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace Barista.Api.Authorization
{
    public class PosExistsRequirementHandler : AuthorizationHandler<PosExistsRequirement>
    {
        private readonly IPointsOfSaleService _posService;

        public PosExistsRequirementHandler(IPointsOfSaleService posService)
        {
            _posService = posService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PosExistsRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == Claims.PointOfSaleId))
            {
                context.Fail();
                return;
            }

            var posId = context.User.GetPointOfSaleId();
            var httpResponse = await _posService.StatPointOfSale(posId);
            if (httpResponse.IsSuccessStatusCode)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
