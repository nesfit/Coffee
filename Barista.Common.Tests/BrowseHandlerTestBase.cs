using AutoMapper;
using Barista.Common.EfCore;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Common.Tests
{
    public abstract class BrowseHandlerTestBase<TQuery, TRepository, TDomain, TDto>
        where TQuery : class, IPagedQueryImpl<TDomain>, IQuery<IPagedResult<TDto>>, new()
        where TRepository : class, IBrowsableRepository<TDomain>
    {
        protected readonly TQuery Query;
        protected readonly TRepository Repository;
        protected readonly IMapper Mapper;
        protected readonly IPagedResult<TDto> ExpectedResult;

        private readonly Mock<TRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IPagedResult<TDomain> _queryResult;

        protected BrowseHandlerTestBase()
        {
            Query = new TQuery();
            ExpectedResult = new Mock<IPagedResult<TDto>>(MockBehavior.Strict).Object;

            _repoMock = new Mock<TRepository>(MockBehavior.Strict);
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);

            _queryResult = new Mock<IPagedResult<TDomain>>(MockBehavior.Strict).Object;
            _repoMock.Setup(r => r.BrowseAsync(Query)).ReturnsAsync(_queryResult).Verifiable();
            _mapperMock.Setup(m => m.Map<IPagedResult<TDto>>(_queryResult)).Returns(ExpectedResult).Verifiable();

            Repository = _repoMock.Object;
            Mapper = _mapperMock.Object;
        }

        protected abstract IQueryHandler<TQuery, IPagedResult<TDto>> InstantiateHandler();

        [TestMethod]
        public void QueriesRepository_ReturnsMappedResult()
        {
            var handler = InstantiateHandler();
            Assert.AreEqual(ExpectedResult, handler.HandleAsync(Query).GetAwaiter().GetResult());
        }

        [TestCleanup]
        private void VerifyExpectations()
        {
            _repoMock.Verify(r => r.BrowseAsync(Query), Times.Once);
            _mapperMock.Verify(m => m.Map<IPagedResult<TDto>>(_queryResult), Times.Once);
        }
    }
}
