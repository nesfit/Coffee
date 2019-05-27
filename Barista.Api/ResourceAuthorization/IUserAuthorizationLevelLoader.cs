using System;
using System.Threading.Tasks;

namespace Barista.Api.ResourceAuthorization
{
    public interface IUserAuthorizationLevelLoader<in TResourceId>
    {
        string ResourceName { get; }
        Task<IUserAuthorizationLevel> LoadUserAuthorizationLevel(Guid userId, TResourceId resourceId);
    }
}
