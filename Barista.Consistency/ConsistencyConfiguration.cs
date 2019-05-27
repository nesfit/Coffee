using System;

namespace Barista.Consistency
{
    public class ConsistencyConfiguration : IConsistencyConfiguration
    {
        public TimeSpan ImmediateRepeatedRunInterval { get; set; }
        public TimeSpan DelayedRepeatedRunInterval { get; set; }
        public TimeSpan ConsistencyTaskLifetime { get; set; }
        public TimeSpan UnconfirmedSaleExpirationInterval { get; set; }
    }
}
