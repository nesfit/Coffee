using Barista.Offers.Controllers;
using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Offers.Dto;
using Barista.Offers.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Offers.Tests.Controllers
{
    [TestClass]
    public class OffersControllerTests
    {
        private readonly OffersController _controller;
        private readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);

        public OffersControllerTests()
        {
            _controller = new OffersController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetOffer_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var offerId = TestIds.A;
            var offerDto = new OfferDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetOffer>(q => q.Id == offerId))).ReturnsAsync(offerDto).Verifiable();

            var actionResult = _controller.GetOffer(offerId).GetAwaiter().GetResult();

            Assert.AreEqual(offerDto, actionResult.Value);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetOffer_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var offerId = TestIds.A;
            var offerDto = (OfferDto) null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetOffer>(q => q.Id == offerId))).ReturnsAsync(offerDto).Verifiable();

            var actionResult = _controller.GetOffer(offerId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseOffers_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseOffers();
            var result = new Mock<IPagedResult<OfferDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseOffers(query).GetAwaiter().GetResult();

            Assert.AreEqual(result, actionResult);
            _dispatcherMock.Verify();
        }
    }
}
