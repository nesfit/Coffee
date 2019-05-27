using System;
using Barista.Accounting.Dto;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetPayment : IQuery<PaymentDto>
    {
        public Guid Id { get; set; }
    }
}
