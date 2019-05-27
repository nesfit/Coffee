using System;
using System.Threading.Tasks;
using Barista.Api.Services;

namespace Barista.Api.ResourceAuthorization.Loaders
{
    public class StockItemAuthorizationLoader : IStockItemAuthorizationLoader
    {
        private readonly IStockItemsService _stockItemsService;
        private readonly IPointOfSaleAuthorizationLoader _posAuthLoader;

        public StockItemAuthorizationLoader(IStockItemsService stockItemsService, IPointOfSaleAuthorizationLoader posAuthLoader)
        {
            _stockItemsService = stockItemsService ?? throw new ArgumentNullException(nameof(stockItemsService));
            _posAuthLoader = posAuthLoader ?? throw new ArgumentNullException(nameof(posAuthLoader));
        }

        public string ResourceName => "Stock item";

        public async Task<IUserAuthorizationLevel> LoadUserAuthorizationLevel(Guid userId, Guid resourceId)
        {
            var stockItem = await _stockItemsService.GetStockItem(resourceId);

            if (stockItem is null)
                return null;
            else
                return await _posAuthLoader.LoadUserAuthorizationLevel(userId, stockItem.PointOfSaleId);
        }
    }
}
