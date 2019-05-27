using System;
using System.Threading.Tasks;

namespace Barista.Mqtt
{
    public interface IPosFeatureVerifier
    {
        Task AssertFeature(Guid posId, string featureName);
    }
}
