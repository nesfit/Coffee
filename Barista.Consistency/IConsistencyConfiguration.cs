using System;

namespace Barista.Consistency
{
    public interface IConsistencyConfiguration
    {
        TimeSpan ImmediateRepeatedRunInterval { get; }
        TimeSpan DelayedRepeatedRunInterval { get; }
        TimeSpan ConsistencyTaskLifetime { get; }
        TimeSpan UnconfirmedSaleExpirationInterval { get; }
    }
}