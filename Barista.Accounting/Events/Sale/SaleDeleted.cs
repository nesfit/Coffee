using System;
using Barista.Contracts.Events.Sale;

namespace Barista.Accounting.Events.Sale
{
    public class SaleDeleted : ISaleDeleted
    {
        public SaleDeleted(Guid id)
        {
            Id = id;
        }

        public SaleDeleted(Domain.Sale sale) : this(sale.Id)
        {

        }

        public Guid Id { get; }
    }
}
