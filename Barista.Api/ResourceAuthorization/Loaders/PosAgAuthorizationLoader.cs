using System;
using System.Threading.Tasks;
using Barista.Api.Services;

namespace Barista.Api.ResourceAuthorization.Loaders
{
    public class PosAgAuthorizationLoader : IPosAgAuthorizationLoader
    {
        private readonly IPointsOfSaleService _posService;
        private readonly IAccountingGroupAuthorizationLoader _agAuthLoader;

        public PosAgAuthorizationLoader(IPointsOfSaleService posService, IAccountingGroupAuthorizationLoader agAuthLoader)
        {
            _posService = posService ?? throw new ArgumentNullException(nameof(posService));
            _agAuthLoader = agAuthLoader ?? throw new ArgumentNullException(nameof(agAuthLoader));
        }

        public string ResourceName => "Point of sale/Accounting group";

        public async Task<IUserAuthorizationLevel> LoadUserAuthorizationLevel(Guid userId, (Guid PointOfSaleId, Guid AccountingGroupId) resourceId)
        {
            var (pointOfSaleId, accountingGroupId) = resourceId;

            var posLevelTask = Task.Run<IUserAuthorizationLevel>(async () => await _posService.GetAuthorizedUser(pointOfSaleId, userId));
            var agLevelTask = Task.Run<IUserAuthorizationLevel>(async () => await _agAuthLoader.LoadUserAuthorizationLevel(userId, accountingGroupId));
            await Task.WhenAll(posLevelTask, agLevelTask);

            var posLevel = posLevelTask.Result;
            var agLevel = agLevelTask.Result;

            if (posLevel != null && agLevel != null)
                return posLevel.CompareTo(agLevel) > 0 ? posLevel : agLevel;
            else
                return posLevel ?? agLevel;
        }
    }
}
