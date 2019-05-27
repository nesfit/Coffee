using AutoMapper;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Barista.Common.Tests
{
    public abstract class GetInnerCollectionHandlerTestBase<THandler, TQuery, TRepository, TDomain, TDomainInner, TDto>
        where THandler : IQueryHandler<TQuery, TDto>
        where TQuery : class, IQuery<TDto>
        where TRepository : class
        where TDomain : class
        where TDomainInner : class
        where TDto : class
    {

        protected abstract THandler InstantiateHandler(TRepository repo, IMapper mapper);
        protected abstract TQuery InstantiateQuery();
        protected abstract Func<TQuery, Expression<Func<TRepository, Task<TDomain>>>> RepositorySetup { get; }

        protected readonly Guid ParentEntityId = Guid.Parse("{C7C93035-D3FA-4D0C-8C42-972A431A24CE}");
        protected readonly Guid InnerEntityId = Guid.Parse("{C7C93035-D3FA-4D0C-8C42-972A431A24CF}");

        [TestMethod]
        public void QueriesRepository_WhenParentEntityIsNull_ThrowsBaristaException_ContainingParentEntityId()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var q = InstantiateQuery();

            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(default(TDomain)).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object);

            var e = Assert.ThrowsException<BaristaException>(() => handler.HandleAsync(q).GetAwaiter().GetResult());
            Assert.IsTrue(e.Code.Contains("not_found"));
            Assert.IsTrue(e.Message.Contains(ParentEntityId.ToString()));
            repoMock.Verify(RepositorySetup(q), Times.Once);
        }

        protected abstract TDomain InstantiateDomainWithoutDesiredEntity(TQuery query);

        protected abstract TDomain InstantiateDomainWithDesiredEntity(TQuery query);

        [TestMethod]
        public void QueriesRepository_ReturnsMappedEntity()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var q = InstantiateQuery();
            var domainObject = InstantiateDomainWithDesiredEntity(q);
            var dtoObject = new Mock<TDto>(MockBehavior.Strict).Object;
            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(domainObject).Verifiable();
            mapperMock.Setup(m => m.Map<TDto>(It.IsNotNull<TDomainInner>())).Returns(dtoObject).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object);
            var result = handler.HandleAsync(q).GetAwaiter().GetResult();

            Assert.AreEqual(dtoObject, result);
            repoMock.Verify(RepositorySetup(q), Times.Once);
            mapperMock.Verify(m => m.Map<TDto>(It.IsNotNull<TDomainInner>()), Times.Once);
        }

        [TestMethod]
        public void QueriesRepository_ReturnsNull_WhenParentEntityDoesNotContainSearchedSubject()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var q = InstantiateQuery();
            var domainObject = InstantiateDomainWithoutDesiredEntity(q);
            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(domainObject).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object);
            var result = handler.HandleAsync(q).GetAwaiter().GetResult();

            Assert.AreEqual(null, result);
            repoMock.Verify(RepositorySetup(q), Times.Once);
        }
    }
}
