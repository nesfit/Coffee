using System;
using System.Threading.Tasks;
using Barista.Accounting.Domain;

namespace Barista.Accounting.Repositories
{
    public interface IBalanceRepository
    {
        Task<Balance> GetAsync(Guid ofUserId);
    }
}