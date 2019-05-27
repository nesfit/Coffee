using AutoMapper;
using Barista.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Barista.Common.Tests
{
    public abstract class GetHandlerTestBase<THandler, TQuery, TRepository, TDomain, TDto>
        where THandler : IQueryHandler<TQuery, TDto>
        where TQuery : class, IQuery<TDto>
        where TRepository : class
        where TDomain : class
        where TDto : class
    {

        protected abstract THandler InstantiateHandler(TRepository repo, IMapper mapper);
        protected abstract TQuery InstantiateQuery();
        protected abstract Func<TQuery, Expression<Func<TRepository, Task<TDomain>>>> RepositorySetup { get; }

        protected const string GuidA = "{C7C93035-D3FA-4D0C-8C42-972A431A24CE}";
        protected const string GuidB = "{C7C93035-D3FA-4D0C-8C42-972A431A24CF}";

        [TestMethod]
        public void QueriesRepository_PropagatesReturnedNullEntity_IntoNullDto()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var q = InstantiateQuery();

            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(default(TDomain)).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object);
            var result = handler.HandleAsync(q).GetAwaiter().GetResult();

            Assert.IsNull(result);
            repoMock.Verify(RepositorySetup(q), Times.Once);
        }

        protected virtual TDomain InstantiateDomain()
        {
            return (TDomain) FormatterServices.GetSafeUninitializedObject(typeof(TDomain));
        }

        [TestMethod]
        public void QueriesRepository_ReturnsMappedEntity()
        {
            var repoMock = new Mock<TRepository>(MockBehavior.Strict);
            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            var domainObject = InstantiateDomain();
            var dtoObject = new Mock<TDto>(MockBehavior.Strict).Object;
            var q = InstantiateQuery();
            repoMock.Setup(RepositorySetup(q)).ReturnsAsync(domainObject).Verifiable();
            mapperMock.Setup(m => m.Map<TDto>(domainObject)).Returns(dtoObject).Verifiable();

            var handler = InstantiateHandler(repoMock.Object, mapperMock.Object);
            var result = handler.HandleAsync(InstantiateQuery()).GetAwaiter().GetResult();

            Assert.AreEqual(dtoObject, result);
            repoMock.Verify(RepositorySetup(q), Times.Once);
            mapperMock.Verify(m => m.Map<TDto>(domainObject), Times.Once);
        }
    }
}
