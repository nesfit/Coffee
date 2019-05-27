using System;
using Barista.Accounting.Domain;
using Barista.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Accounting.Tests.Domain
{
    [TestClass]
    public class SaleTests
    {
        [TestMethod]
        public void SetCost_RejectsNegativeNumbers()
        {
            foreach (var cost in new[] { -5m, -0.01m})
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new Sale(Guid.Empty, cost, 1, Guid.Empty, Guid.Empty,
                    Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty));

                Assert.AreEqual("invalid_cost", ex.Code);
            }
        }

        [TestMethod]
        public void SetQuantity_RejectsNegativeNumbersAndZero()
        {
            foreach (var quantity in new[] { -5m, -0.01m, 0m })
            {
                var ex = Assert.ThrowsException<BaristaException>(() => new Sale(Guid.Empty, 1, quantity, Guid.Empty, Guid.Empty,
                    Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty));

                Assert.AreEqual("invalid_quantity", ex.Code);
            }
        }

        [TestMethod]
        public void AddSaleStateChange_RejectsNullValue()
        {
            var sale = new Sale(Guid.Empty, 1, 1, Guid.Empty, Guid.Empty,
                Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);

            var ex = Assert.ThrowsException<BaristaException>(() => sale.AddStateChange(null));
            Assert.AreEqual("invalid_sale_state_change", ex.Code);
        }

        private static readonly Func<SaleStateChange>[] StateChangeTestData = new Func<SaleStateChange>[]
        {
            () => new SaleStateChange(Guid.Empty, "Reason", SaleState.Cancelled, null, null),
            () => new SaleStateChange(Guid.Empty, "Reason", SaleState.FundsReserved, null, null),
            () => new SaleStateChange(Guid.Empty, "Reason", SaleState.Confirmed, null, null)
        };

        [TestMethod]
        public void AddSaleStateChange_OnlyAcceptsFundsReservedState_WhenInInitialState_SaleStateChangesWhenSuccessful()
        {
            foreach (var changeFactory in StateChangeTestData)
            {
                var sale = new Sale(Guid.Empty, 1, 1, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                var change = changeFactory();
                void Act() => sale.AddStateChange(change);

                switch (change.State)
                {
                    case SaleState.FundsReserved:
                        Act(); // must not throw
                        Assert.AreEqual(SaleState.FundsReserved, sale.State);
                        continue;

                    case SaleState.Cancelled:
                    case SaleState.Confirmed:
                        var ex = Assert.ThrowsException<BaristaException>((Action) Act);
                        Assert.AreEqual("invalid_sale_state_transition", ex.Code);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        [TestMethod]
        public void AddSaleStateChange_DoesNotAcceptAnyNewState_WhenInCancelledState()
        {
            var sale = new Sale(Guid.Empty, 1, 1, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
            sale.StateChanges.Add(new SaleStateChange(Guid.Empty, "Reason", SaleState.Cancelled, null, null));
            Assert.AreEqual(SaleState.Cancelled, sale.State);

            foreach (var changeFactory in StateChangeTestData)
            {
                var change = changeFactory();
                void Act() => sale.AddStateChange(change);
                var ex = Assert.ThrowsException<BaristaException>((Action)Act);
                Assert.AreEqual("invalid_sale_state_transition", ex.Code);
            }
        }

        [TestMethod]
        public void AddSaleStateChange_AcceptsConfirmedAndCancelled_WhenInFundsReservedState_SaleStateChangesWhenSuccessful()
        {
            foreach (var changeFactory in StateChangeTestData)
            {
                var sale = new Sale(Guid.Empty, 1, 1, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                sale.StateChanges.Add(new SaleStateChange(Guid.Empty, "Reason", SaleState.FundsReserved, null, null));
                Assert.AreEqual(SaleState.FundsReserved, sale.State);

                var change = changeFactory();
                void Act() => sale.AddStateChange(change);

                switch (change.State)
                {
                    case SaleState.FundsReserved:
                        var ex = Assert.ThrowsException<BaristaException>((Action) Act);
                        Assert.AreEqual("invalid_sale_state_transition", ex.Code);
                        break;

                    case SaleState.Confirmed:
                        Act(); // must not throw
                        Assert.AreEqual(SaleState.Confirmed, sale.State);
                        break;

                    case SaleState.Cancelled:
                        Act(); // must not throw
                        Assert.AreEqual(SaleState.Cancelled, sale.State);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        [TestMethod]
        public void AddSaleStateChange_AcceptsCancelled_WhenInConfirmedState_SaleStateChangesWhenSuccessful()
        {
            foreach (var changeFactory in StateChangeTestData)
            {
                var sale = new Sale(Guid.Empty, 1, 1, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                sale.StateChanges.Add(new SaleStateChange(Guid.Empty, "Reason", SaleState.Confirmed, null, null));
                Assert.AreEqual(SaleState.Confirmed, sale.State);

                var change = changeFactory();
                void Act() => sale.AddStateChange(change);

                switch (change.State)
                {
                    case SaleState.Cancelled:
                        Act(); // must not throw
                        Assert.AreEqual(SaleState.Cancelled, sale.State);
                        break;

                    case SaleState.FundsReserved:
                    case SaleState.Confirmed:
                        var ex = Assert.ThrowsException<BaristaException>((Action) Act);
                        Assert.AreEqual("invalid_sale_state_transition", ex.Code);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
