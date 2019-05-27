using Barista.Common.EfCore;
using Barista.Offers.Domain;

namespace Barista.Offers.Repositories
{
    public class OfferRepository : CrudRepository<OffersDbContext, Offer>, IOfferRepository
    {
        public OfferRepository(OffersDbContext dbContext) : base(dbContext, dbc => dbc.Offers)
        {
        }
    }
}
