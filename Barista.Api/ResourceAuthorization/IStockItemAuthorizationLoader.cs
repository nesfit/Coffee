using System;

namespace Barista.Api.ResourceAuthorization
{
    public interface IStockItemAuthorizationLoader : IUserAuthorizationLevelLoader<Guid>
    {
    }
}
