using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Common.Tests
{
    public abstract class BrowseQueryTestBase<TQueryImpl, TDomain> where TQueryImpl : IPagedQueryImpl<TDomain>
    {
        public class QueryOption
        {
            public string Name { get; set; }
            public Action<TQueryImpl> QueryConfiguratorAction { get; set; }
            public IEnumerable<TDomain> SampleData { get; set; }
            public Action<IEnumerable<TDomain>> SampleDataValidator { get; set; }
        }

        protected abstract QueryOption[] Options
        {
            get;
        }

        protected abstract TQueryImpl InstantiateQuery();

        [TestMethod]
        public void OptionsPerformValidLookups()
        {
            foreach (var option in Options)
            {
                var query = InstantiateQuery();
                option.QueryConfiguratorAction(query);

                var resultData = query.Apply(option.SampleData.AsQueryable());
                option.SampleDataValidator(resultData);
            }
        }
    }
}
