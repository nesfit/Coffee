using System.Linq;
using Barista.Common.EfCore;
using Barista.PointsOfSale.Domain;
using Microsoft.EntityFrameworkCore;

namespace Barista.PointsOfSale.Repositories
{
    public class PointOfSaleRepository : CrudRepository<PointsOfSaleDbContext, PointOfSale>, IPointOfSaleRepository
    {
        public PointOfSaleRepository(PointsOfSaleDbContext dbContext) : base(dbContext, dbc => dbc.PointsOfSale)
        {

        }

        protected override IQueryable<PointOfSale> QueryableAccessorFunc()
        {
            return base.QueryableAccessorFunc().Include(pos => pos.KeyValues);
        }
    }
}
