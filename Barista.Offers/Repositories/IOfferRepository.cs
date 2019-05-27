using System;
using System.Threading.Tasks;
using Barista.Common.EfCore;
using Barista.Offers.Domain;

namespace Barista.Offers.Repositories
{
    public interface IOfferRepository : IBrowsableRepository<Offer>
    {
        Task<Offer> GetAsync(Guid id);
        Task AddAsync(Offer offer);
        Task UpdateAsync(Offer offer);
        Task DeleteAsync(Offer offer);
        Task SaveChanges();
    }
}
