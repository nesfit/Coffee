using System;
using System.Collections.Generic;
using Barista.Common;
using Barista.Contracts;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.SaleStrategies.Tests.Repositories
{
    [TestClass]
    public class SaleStrategyRepositoryTests
    {
        private readonly Mock<IClientsidePaginator<SaleStrategy>> _paginatorMock;
        private readonly SaleStrategyRepository _repository;

        public SaleStrategyRepositoryTests()
        {
            _paginatorMock = new Mock<IClientsidePaginator<SaleStrategy>>(MockBehavior.Strict);
            _repository = new SaleStrategyRepository(_paginatorMock.Object);
        }

        [TestMethod]
        public void GetCreditStrategy_ReturnsInstance()
        {
            Assert.IsInstanceOfType(_repository.GetSaleStrategy(CreditStrategy.StaticId).GetAwaiter().GetResult(), typeof(CreditStrategy));
        }

        [TestMethod]
        public void GetUnlimitedStrategy_ReturnsInstance()
        {
            Assert.IsInstanceOfType(_repository.GetSaleStrategy(UnlimitedStrategy.StaticId).GetAwaiter().GetResult(), typeof(UnlimitedStrategy));
        }

        [TestMethod]
        public void GetUnknownStrategy_ReturnsNull()
        {
            Assert.IsNull(_repository.GetSaleStrategy(Guid.Empty).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void BrowseSaleStrategies_AppliesPaginator_ReturnsItsResults()
        {
            var result = new Mock<IPagedResult<SaleStrategy>>(MockBehavior.Strict).Object;
            var queryMock = new Mock<IPagedQueryImpl<SaleStrategy>>(MockBehavior.Strict);

            _paginatorMock.Setup(p => p.PaginateAsync(It.IsNotNull<ICollection<SaleStrategy>>(), queryMock.Object))
                .ReturnsAsync(result).Verifiable();

            Assert.AreEqual(result, _repository.BrowseAsync(queryMock.Object).GetAwaiter().GetResult());

            _paginatorMock.Verify();
        }
    }
}
