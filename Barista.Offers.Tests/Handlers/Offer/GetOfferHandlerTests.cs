using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Offers.Dto;
using Barista.Offers.Handlers.Offer;
using Barista.Offers.Queries;
using Barista.Offers.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Offers.Tests.Handlers.Offer
{
    [TestClass]
    public class GetOfferHandlerTests : GetHandlerTestBase<GetOfferHandler, GetOffer, IOfferRepository, Domain.Offer, OfferDto>
    {
        protected override GetOfferHandler InstantiateHandler(IOfferRepository repo, IMapper mapper)
            => new GetOfferHandler(repo, mapper);

        protected override GetOffer InstantiateQuery()
            => new GetOffer(TestIds.A);

        protected override Func<GetOffer, Expression<Func<IOfferRepository, Task<Domain.Offer>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
