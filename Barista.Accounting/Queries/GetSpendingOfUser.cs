using System;
using System.Linq;
using Barista.Accounting.Domain;
using Barista.Contracts;

namespace Barista.Accounting.Queries
{
    public class GetSpendingOfUser : IQuery<decimal>
    {
        public Guid UserId { get; set; }
        public DateTimeOffset? Since { get; set; }
        public DateTimeOffset? Until { get; set; }
        public Guid? AtPointOfSaleId { get; set; }
        public Guid? AtAccountingGroupId { get; set; }
        public bool IncludePayments { get; set; } = true;

        public IQueryable<SpendingOfUsers> Apply(IQueryable<SpendingOfUsers> query)
        {
            query = query.Where(sp => sp.UserId == UserId);

            if (Since is DateTimeOffset since)
                query = query.Where(sp => sp.Created >= since);

            if (Until is DateTimeOffset until)
                query = query.Where(sp => sp.Created <= until);

            if (!IncludePayments)
                query = query.Where(sp => sp.PaymentId == null);

            if (AtPointOfSaleId is Guid saleAtPos)
                query = query.Where(sp => sp.PointOfSaleId == null || sp.PointOfSaleId == saleAtPos);

            if (AtAccountingGroupId is Guid saleAtAg)
                query = query.Where(sp => sp.AccountingGroupId == null || sp.AccountingGroupId == saleAtAg);

            return query.OrderBy(sp => sp.Created);
        }
    }
}
