using System;
using Barista.Accounting.Dto;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetBalance : IQuery<BalanceDto>
    {
        public Guid UserId { get; }

        public GetBalance(Guid userId)
        {
            UserId = userId;
        }
    }
}
