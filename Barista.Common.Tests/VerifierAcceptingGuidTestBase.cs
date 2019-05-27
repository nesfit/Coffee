using System;

namespace Barista.Common.Tests
{
    public abstract class VerifierAcceptingGuidTestBase<TService> : VerifierTestBase<Guid, TService> where TService : class
    {
        protected override Guid ResourceId => TestIds.A;
    }
}
