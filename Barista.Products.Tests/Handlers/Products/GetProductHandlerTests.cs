using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Products.Domain;
using Barista.Products.Dto;
using Barista.Products.Handlers.Product;
using Barista.Products.Queries;
using Barista.Products.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Products.Tests.Handlers.Products
{
    [TestClass]
    public class GetProductHandlerTests : GetHandlerTestBase<GetProductHandler, GetProduct, IProductRepository, Product, ProductDto>
    {
        protected override GetProductHandler InstantiateHandler(IProductRepository repo, IMapper mapper)
            => new GetProductHandler(repo, mapper);

        protected override GetProduct InstantiateQuery()
            => new GetProduct(TestIds.A);

        protected override Func<GetProduct, Expression<Func<IProductRepository, Task<Product>>>> RepositorySetup
            => query => repo => repo.GetAsync(TestIds.A);
    }
}
