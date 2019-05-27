using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Domain;

namespace Barista.Identity.Repositories
{
    public interface IAssignedMeansRepository
    {
        Task<IPagedResult<AuthenticationMeans>> BrowseMeansAssignedToUser(Guid userId, IPagedQueryImpl<AuthenticationMeans> query);
        Task<IPagedResult<AuthenticationMeans>> BrowseMeansAssignedToPointOfSale(Guid posId, IPagedQueryImpl<AuthenticationMeans> query);
    }
}
