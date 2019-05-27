using System;
using System.Threading.Tasks;
using Barista.Api.Services;

namespace Barista.Api.ResourceAuthorization.Loaders
{
    public class PointOfSaleAuthorizationLoader : IPointOfSaleAuthorizationLoader
    {
        private readonly IPointsOfSaleService _pointsOfSaleService;
        private readonly IPosAgAuthorizationLoader _posAgAuthLoader;

        public PointOfSaleAuthorizationLoader(IPointsOfSaleService pointsOfSaleService, IPosAgAuthorizationLoader posAgAuthLoader)
        {
            _pointsOfSaleService = pointsOfSaleService ?? throw new ArgumentNullException(nameof(pointsOfSaleService));
            _posAgAuthLoader = posAgAuthLoader;
        }

        public string ResourceName => "Point of sale";

        public async Task<IUserAuthorizationLevel> LoadUserAuthorizationLevel(Guid userId, Guid posId)
        {
            var pos = await _pointsOfSaleService.GetPointOfSale(posId);
            if (pos is null)
                return null;

            return await _posAgAuthLoader.LoadUserAuthorizationLevel(userId, (posId, pos.ParentAccountingGroupId));
        }
    }
}
