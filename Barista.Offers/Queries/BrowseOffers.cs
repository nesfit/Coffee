using System;
using System.Linq;
using Barista.Common;
using Barista.Offers.Domain;
using Barista.Offers.Dto;

namespace Barista.Offers.Queries
{
    public class BrowseOffers : PagedQuery<OfferDto>, IPagedQueryImpl<Offer>
    {
        public Guid? AtPointOfSaleId { get; set; }
        public Guid? OfProductId { get; set; }
        public Guid? OfStockItemId { get; set; }
        public DateTimeOffset? ValidAt { get; set; }

        public IQueryable<Offer> Apply(IQueryable<Offer> q)
        {
            q = q.ApplySort(SortBy);

            if (AtPointOfSaleId is Guid posId)
                q = q.Where(o => o.PointOfSaleId == posId);

            if (OfProductId is Guid prodId)
                q = q.Where(o => o.ProductId == prodId);

            if (OfStockItemId is Guid siId)
                q = q.Where(o => o.StockItemId == siId);

            if (ValidAt is DateTimeOffset validAt)
                q = q.Where(o => o.ValidSince == null || o.ValidSince < validAt)
                     .Where(o => o.ValidUntil == null || o.ValidUntil > validAt);

            return q;
        }
    }
}
