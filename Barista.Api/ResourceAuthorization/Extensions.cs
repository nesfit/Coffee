using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.ResourceAuthorization.Loaders;

namespace Barista.Api.ResourceAuthorization
{
    public static class Extensions
    {
        public static async Task AssertResourceAccessAsync<TResourceId>(this IUserAuthorizationLevelLoader<TResourceId> loader, ClaimsPrincipal user, TResourceId resourceId, IUserAuthorizationLevelPolicy policy)
        {
            if (loader == null) throw new ArgumentNullException(nameof(loader));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (resourceId == null) throw new ArgumentNullException(nameof(resourceId));
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            var userId = user.GetUserId();
            var userLevelResult = await loader.LoadUserAuthorizationLevel(userId, resourceId);
            var userLevel = userLevelResult?.Level;

            if (userLevelResult != null && policy.IsSatisfied(userLevel))
                return;
            else if (user.HasClaim(Claims.IsAdministrator, true.ToString()))
                return; // TODO: log elevation of privileges
            else
                throw new ResourceAuthorizationFailedException(loader.ResourceName, policy.RequiredLevel, resourceId);
        }

        public static async Task AssertMultipleResourceAccessAsync<TResourceId>(this IUserAuthorizationLevelLoader<TResourceId> loader, ClaimsPrincipal user, TResourceId[] resourceIds, IUserAuthorizationLevelPolicy policy)
        {
            if (loader == null) throw new ArgumentNullException(nameof(loader));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (resourceIds == null) throw new ArgumentNullException(nameof(resourceIds));
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            if (resourceIds is null)
                return;

            var userId = user.GetUserId();
            var tasks = resourceIds.ToDictionary(
                resId => resId, resourceId =>
                Task.Run(() => loader.LoadUserAuthorizationLevel(userId, resourceId))
            );

            await Task.WhenAll(tasks.Values);

            foreach (var task in tasks)
            {
                var userLevel = task.Value?.Result?.Level;

                if (userLevel != null && policy.IsSatisfied(userLevel))
                    return;
                else if (user.HasClaim(Claims.IsAdministrator, true.ToString())) // TODO: :(
                    return; // TODO: log elevation of privileges
                else
                    throw new ResourceAuthorizationFailedException(loader.ResourceName, policy.RequiredLevel, task.Key);
            }
        }
    }
}
