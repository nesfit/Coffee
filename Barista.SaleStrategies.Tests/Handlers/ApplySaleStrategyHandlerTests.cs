using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts.Commands.SaleStrategy;
using Barista.SaleStrategies.Domain;
using Barista.SaleStrategies.Handlers.SaleStrategy;
using Barista.SaleStrategies.Repositories;
using Barista.SaleStrategies.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Barista.SaleStrategies.Tests.Handlers
{
    [TestClass]
    public class ApplySaleStrategyHandlerTests
    {
        private readonly Mock<ISaleStrategyRepository> _repoMock = new Mock<ISaleStrategyRepository>(MockBehavior.Strict);
        private readonly Mock<IAccountingService> _accSvcMock = new Mock<IAccountingService>(MockBehavior.Strict);
        private readonly Mock<IApplySaleStrategy> _cmdMock = new Mock<IApplySaleStrategy>(MockBehavior.Strict);

        private readonly ICorrelationContext _context = new Mock<ICorrelationContext>(MockBehavior.Strict).Object;

        private IApplySaleStrategy Cmd => _cmdMock.Object;

        private static readonly Guid UserId = TestIds.A;
        private static readonly Guid SaleStrategyId = TestIds.B;
        private const decimal Cost = 42;

        public ApplySaleStrategyHandlerTests()
        {
            _cmdMock.SetupGet(c => c.UserId).Returns(UserId);
            _cmdMock.SetupGet(c => c.SaleStrategyId).Returns(SaleStrategyId);
            _cmdMock.SetupGet(c => c.Cost).Returns(Cost);
        }

        private class AlwaysFailsStrategy : SaleStrategy
        {
            public AlwaysFailsStrategy() : base(TestIds.A, "AlwaysFails")
            {
            }

            public int InvocationCount { get; private set; }

            public override Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
            {
                Assert.IsNotNull(accountingService);
                Assert.AreEqual(UserId, userId);
                Assert.AreEqual(Cost, cost);
                InvocationCount++;
                return Task.FromResult(false);
            }
        }

        private class AlwaysSucceedsStrategy : SaleStrategy
        {
            public AlwaysSucceedsStrategy() : base(TestIds.B, "AlwaysSucceeds")
            {
            }

            public int InvocationCount { get; private set; }

            public override Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost)
            {
                Assert.IsNotNull(accountingService);
                Assert.AreEqual(UserId, userId);
                Assert.AreEqual(Cost, cost);
                InvocationCount++;
                return Task.FromResult(true);
            }
        }

        [TestMethod]
        public void WhenSaleStrategyDoesNotExist_Fails()
        {
            _repoMock.Setup(r => r.GetSaleStrategy(SaleStrategyId)).ReturnsAsync((SaleStrategy)null);

            var handler = new ApplySaleStrategyHandler(_repoMock.Object, _accSvcMock.Object);
            var result = handler.HandleAsync(Cmd, _context).GetAwaiter().GetResult();

            Assert.IsFalse(result.Successful);
            Assert.AreEqual("sale_strategy_not_found", result.ErrorCode);
        }

        [TestMethod]
        public void WhenSaleStrategyApplicationFails_Fails()
        {
            var strategy = new AlwaysFailsStrategy();
            _repoMock.Setup(r => r.GetSaleStrategy(SaleStrategyId)).ReturnsAsync(strategy);

            var handler = new ApplySaleStrategyHandler(_repoMock.Object, _accSvcMock.Object);
            var result = handler.HandleAsync(Cmd, _context).GetAwaiter().GetResult();

            Assert.IsFalse(result.Successful);
            Assert.AreEqual("sale_strategy_application_failed", result.ErrorCode);
            Assert.AreEqual(1, strategy.InvocationCount);
        }

        [TestMethod]
        public void WhenSaleStrategyApplicationSucceeds_Succeeds()
        {
            var strategy = new AlwaysSucceedsStrategy();
            _repoMock.Setup(r => r.GetSaleStrategy(SaleStrategyId)).ReturnsAsync(strategy);

            var handler = new ApplySaleStrategyHandler(_repoMock.Object, _accSvcMock.Object);
            var result = handler.HandleAsync(Cmd, _context).GetAwaiter().GetResult();

            Assert.IsTrue(result.Successful);
            Assert.AreEqual(1, strategy.InvocationCount);
        }
    }
}
