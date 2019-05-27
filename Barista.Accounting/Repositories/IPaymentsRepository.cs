using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Common;
using Barista.Common.EfCore;
using Barista.Contracts;

namespace Barista.Accounting.Repositories
{
    public interface IPaymentsRepository : IBrowsableRepository<Payment>
    {
        Task<Payment> GetAsync(Guid id);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task SaveChanges();
    }
}
