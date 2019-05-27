using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Mqtt.Services;

namespace Barista.Mqtt
{
    public class PosFeatureVerifier : IPosFeatureVerifier
    {
        private readonly IPointsOfSaleService _service;

        public PosFeatureVerifier(IPointsOfSaleService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task AssertFeature(Guid posId, string featureName)
        {
            var pos = await _service.GetPointOfSale(posId);
            if (pos is null)
                throw new BaristaException("point_of_sale_not_found", $"Could not find point of sale with ID '{posId}'");

            if (!pos.Features.Contains(featureName))
                throw new BaristaException("invalid_point_of_sale_features", $"The point of sale with ID '{posId}' is not configured with the required feature of '{featureName}'");
        }
    }
}
