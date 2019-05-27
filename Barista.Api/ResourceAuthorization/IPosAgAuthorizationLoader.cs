using System;

namespace Barista.Api.ResourceAuthorization
{
    public interface IPosAgAuthorizationLoader : IUserAuthorizationLevelLoader<(Guid PointOfSaleId, Guid AccountingGroupId)>
    {
    }
}
