using System;
using Barista.Common.Tests;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Models;
using Barista.SaleStrategies.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.SaleStrategies.Tests.DomainObjects
{
    [TestClass]
    public class CreditStrategyTests
    {
        private readonly CreditStrategy _strategy = new CreditStrategy();
        private readonly Mock<IAccountingService> _accServiceMock;
        private static readonly Guid UserId = TestIds.A;
        private const decimal Cost = 5;

        public CreditStrategyTests()
        {
            _accServiceMock = new Mock<IAccountingService>(MockBehavior.Strict);
        }

        protected void SetupUserBalance(decimal balance)
        {
            _accServiceMock.Setup(svc => svc.GetBalance(UserId)).ReturnsAsync(new UserBalance { Value = balance });
        }

        [TestMethod]
        public void HasExpectedId()
        {
            Assert.AreEqual(Guid.Parse("CAFE0000-0055-0000-0000-00000000000C"), _strategy.Id);
        }

        [TestMethod]
        public void HasExpectedDisplayName()
        {
            Assert.AreEqual("Credit", _strategy.DisplayName);
        }

        [TestMethod]
        public void PermitsSpendingIntoPositiveBalance()
        {
            SetupUserBalance(Cost + 5);
            var result = _strategy.ApplyAsync(_accServiceMock.Object, UserId, Cost).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PermitsSpendingIntoZeroBalance()
        {
            SetupUserBalance(Cost);
            var result = _strategy.ApplyAsync(_accServiceMock.Object, UserId, Cost).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ForbidsSpendingIntoNegativeBalance()
        {
            SetupUserBalance(0);
            var result = _strategy.ApplyAsync(_accServiceMock.Object, UserId, Cost).GetAwaiter().GetResult();
            Assert.IsFalse(result);
        }
    }
}
