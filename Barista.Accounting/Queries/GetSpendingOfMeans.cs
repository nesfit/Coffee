using System;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetSpendingOfMeans : IQuery<decimal>
    {
        public GetSpendingOfMeans(Guid authenticationMeansId, DateTimeOffset? since)
        {
            AuthenticationMeansId = authenticationMeansId;
            Since = since;
        }

        public Guid AuthenticationMeansId { get; }
        public DateTimeOffset? Since { get; }
    }
}
