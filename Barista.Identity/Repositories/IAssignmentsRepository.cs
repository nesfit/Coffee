using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public interface IAssignmentsRepository : IBrowsableRepository<Assignment>, IBrowsableRepository<AssignmentToUser>, IBrowsableRepository<AssignmentToPointOfSale>
    {
        Task<Assignment> GetAsync(Guid id);
        Task AddAsync(Assignment authenticationMeans);
        Task UpdateAsync(Assignment authenticationMeans);
        Task DeleteAsync(Assignment authenticationMeans);
        Task SaveChanges();
    }
}
