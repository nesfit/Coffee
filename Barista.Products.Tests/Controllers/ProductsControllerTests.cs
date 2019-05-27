using Barista.Common.Dispatchers;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Products.Controllers;
using Barista.Products.Dto;
using Barista.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Products.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        readonly Mock<IDispatcher> _dispatcherMock = new Mock<IDispatcher>(MockBehavior.Strict);
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _controller = new ProductsController(_dispatcherMock.Object);
        }

        [TestMethod]
        public void GetProduct_ConstructsQuery_ReturnsResultOfDispatch()
        {
            var productId = TestIds.A;
            var result = new ProductDto();
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetProduct>(q => q.Id == productId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetProduct(productId).GetAwaiter().GetResult();

            Assert.AreEqual(actionResult.Value, result);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void GetProduct_ConstructsQuery_WhenResultOfDispatchIsNull_ReturnsNotFound()
        {
            var productId = TestIds.A;
            var result = (ProductDto)null;
            _dispatcherMock.Setup(d => d.QueryAsync(It.Is<GetProduct>(q => q.Id == productId))).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.GetProduct(productId).GetAwaiter().GetResult();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
            _dispatcherMock.Verify();
        }

        [TestMethod]
        public void BrowseProducts_UsesQuery_ReturnsResultOfDispatch()
        {
            var query = new BrowseProducts();
            var result = new Mock<IPagedResult<ProductDto>>(MockBehavior.Strict).Object;
            _dispatcherMock.Setup(d => d.QueryAsync(query)).ReturnsAsync(result).Verifiable();

            var actionResult = _controller.BrowseProducts(query).GetAwaiter().GetResult();
            Assert.AreEqual(result, actionResult);

            _dispatcherMock.Verify();
        }
    }
}
