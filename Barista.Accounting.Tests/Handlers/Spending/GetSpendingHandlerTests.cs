using System;
using Barista.Accounting.Handlers.Spending;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.Accounting.Tests.Handlers.Spending
{
    [TestClass]
    public class GetSpendingHandlerTests
    {
        private static void TestHandlerWithQueryParams(Guid meansId, DateTimeOffset? since, decimal spendingToReturn)
        {
            var repoMock = new Mock<ISpendingRepository>(MockBehavior.Strict);
            repoMock.Setup(r => r.GetSpendingOfMeans(meansId, since)).ReturnsAsync(spendingToReturn).Verifiable();
            var handler = new GetSpendingOfMeansHandler(repoMock.Object);
            var query = new GetSpendingOfMeans(meansId, since);

            var spending = handler.HandleAsync(query).GetAwaiter().GetResult();

            Assert.AreEqual(spendingToReturn, spending);
            repoMock.Verify(r => r.GetSpendingOfMeans(meansId, since));
        }

        [TestMethod]
        public void WhenSinceIsNull_RetrievesSpendingFromRepository_ReturnsSum()
            => TestHandlerWithQueryParams(TestIds.A, default(DateTimeOffset?), 100m);

        [TestMethod]
        public void WhenSinceIsNotNull_RetrievesSpendingFromRepository_ReturnsSum()
            => TestHandlerWithQueryParams(TestIds.B, new DateTimeOffset(TestDateTimes.Year2001), 200m);
    }
}
