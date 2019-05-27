using AutoMapper;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Barista.Common.Tests
{
    public abstract class BrowseInnerCollectionHandlerTestBase<THandler, TQuery, TRepository, TDomain, TDomainInner, TDto>
        where THandler : IQueryHandler<TQuery, IPagedResult<TDto>>
        where TQuery : class, IPagedQueryImpl<TDomainInner>, IQuery<IPagedResult<TDto>>
        where TRepository : class
        where TDomain : class
        where TDomainInner : class
        where TDto : class
    {

        protected abstract THandler InstantiateHandler(TRepository repo, IMapper mapper, IClientsidePaginator<TDomainInner> paginator);
        protected abstract TQuery InstantiateQuery();
        protected abstract Func<TQuery, Expression<Func<TRepository, Task<TDomain>>>> RepositorySetup { get; }

        protected readonly Guid ParentEntityId = Guid.Parse("{C7C93035-D3FA-4D0C-8C42-972A431A24CE}");

        [TestMethod]
        public void QueriesRepository_WhenParentEntityIsNull_ThrowsBaristaException_ContainingParentEntityId()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var paginatorMock = new Mock<IClientsidePaginator<TDomainInner>>(MockBehavior.Strict);
            var q = InstantiateQuery();

            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(default(TDomain)).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object, paginatorMock.Object);

            var e = Assert.ThrowsException<BaristaException>(() => handler.HandleAsync(q).GetAwaiter().GetResult());
            Assert.IsTrue(e.Code.Contains("not_found"));
            Assert.IsTrue(e.Message.Contains(ParentEntityId.ToString()));
            repoMock.Verify(RepositorySetup(q), Times.Once);
        }

        protected abstract TDomain InstantiateDomain(TQuery query, out ICollection<TDomainInner> innerCollection);

        [TestMethod]
        public void LocatesParentEntity_PaginatesInnerCollection_ReturnsMappedPagedResult()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var paginatorMock = new Mock<IClientsidePaginator<TDomainInner>>(MockBehavior.Strict);
            var q = InstantiateQuery();
            var domainObject = InstantiateDomain(q, out var assignedCollection);
            var mockFinalResult = new Mock<IPagedResult<TDomainInner>>(MockBehavior.Strict);
            var mockMappedResult = new Mock<IPagedResult<TDto>>(MockBehavior.Strict);

            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(domainObject).Verifiable();
            paginatorMock.Setup(p => p.PaginateAsync(assignedCollection, q)).ReturnsAsync(mockFinalResult.Object).Verifiable();
            mapperMock.Setup(m => m.Map<IPagedResult<TDto>>(mockFinalResult.Object)).Returns(mockMappedResult.Object).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object, paginatorMock.Object);
            var result = handler.HandleAsync(q).GetAwaiter().GetResult();

            Assert.AreEqual(mockMappedResult.Object, result);
            repoMock.Verify(RepositorySetup(q), Times.Once);
            paginatorMock.Verify(p => p.PaginateAsync(assignedCollection, q), Times.Once);
            mapperMock.Verify(m => m.Map<IPagedResult<TDto>>(mockFinalResult.Object), Times.Once);
        }
    }
}
