using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Common.EfCore;

namespace Barista.Accounting.Repositories
{
    public interface ISpendingRepository : IBrowsableRepository<SpendingOfUsers>
    {
        Task<decimal> GetSpendingOfMeans(Guid authenticationMeansId, DateTimeOffset? since);
        Task<decimal> GetSpendingOfUser(GetSpendingOfUser query);
    }
}