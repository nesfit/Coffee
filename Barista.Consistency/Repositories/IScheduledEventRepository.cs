using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Consistency.Domain;

namespace Barista.Consistency.Repositories
{
    public interface IScheduledEventRepository : IBrowsableRepository<ScheduledEvent>
    {
        Task<ScheduledEvent> GetAsync(Guid id);
        Task AddAsync(ScheduledEvent msg);
        Task DeleteAsync(ScheduledEvent msg);
        Task SaveChanges();
    }
}
