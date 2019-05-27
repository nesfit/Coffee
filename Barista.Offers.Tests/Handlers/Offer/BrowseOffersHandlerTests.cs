using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Offers.Dto;
using Barista.Offers.Handlers.Offer;
using Barista.Offers.Queries;
using Barista.Offers.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Offers.Tests.Handlers.Offer
{
    [TestClass]
    public class BrowseOffersHandlerTests : BrowseHandlerTestBase<BrowseOffers, IOfferRepository, Domain.Offer, OfferDto>
    {
        protected override IQueryHandler<BrowseOffers, IPagedResult<OfferDto>> InstantiateHandler()
            => new BrowseOffersHandler(Repository, Mapper);
    }
}
