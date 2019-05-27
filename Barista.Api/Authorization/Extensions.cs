using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Barista.Api.Authorization
{
    public static class Extensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userIdClaim = user.Claims.SingleOrDefault(c => c.Type == Claims.UserId);
            if (userIdClaim == null)
                throw new InvalidOperationException($"The signed-in user does not have a claim named '{Claims.UserId}'");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new InvalidOperationException($"Could not parse the following user ID claim value as GUID: '{userIdClaim.Value}'");

            return userId;
        }

        public static Guid GetMeansId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var meansIdClaim = user.Claims.SingleOrDefault(c => c.Type == Claims.MeansId);
            if (meansIdClaim == null)
                throw new InvalidOperationException($"The signed-in user does not have a claim named '{Claims.MeansId}'");

            if (!Guid.TryParse(meansIdClaim.Value, out var meansId))
                throw new InvalidOperationException($"Could not parse the following means ID claim value as GUID: '{meansIdClaim.Value}'");

            return meansId;
        }

        public static Guid GetPointOfSaleId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var posIdClaim = user.Claims.SingleOrDefault(c => c.Type == Claims.PointOfSaleId);
            if (posIdClaim == null)
                throw new InvalidOperationException($"The signed-in user does not have a claim named '{Claims.PointOfSaleId}'");

            if (!Guid.TryParse(posIdClaim.Value, out var posId))
                throw new InvalidOperationException($"Could not parse the following PoS ID claim value as GUID: '{posIdClaim.Value}'");

            return posId;
        }

        public static bool IsPointOfSale(this ClaimsPrincipal pos)
        {
            if (pos == null) throw new ArgumentNullException(nameof(pos));
            return pos.Claims.SingleOrDefault(c => c.Type == Claims.PointOfSaleId) != null;
        }

        public static async Task<bool> IsAdministrator(this IAuthorizationService authService, ClaimsPrincipal user)
        {
            if (authService == null) throw new ArgumentNullException(nameof(authService));
            if (user == null) throw new ArgumentNullException(nameof(user));

            var authResult = await authService.AuthorizeAsync(user, Policies.IsAdministrator);
            return authResult.Succeeded;
        }

        public static bool HasAdministratorClaim(this ClaimsPrincipal user)
        {
            return user.HasClaim(Claims.IsAdministrator, true.ToString());
        }
    }
}
