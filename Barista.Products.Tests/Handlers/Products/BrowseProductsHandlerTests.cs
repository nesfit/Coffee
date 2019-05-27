using Barista.Common;
using Barista.Common.Tests;
using Barista.Contracts;
using Barista.Products.Domain;
using Barista.Products.Dto;
using Barista.Products.Handlers.Product;
using Barista.Products.Queries;
using Barista.Products.Repositories;

namespace Barista.Products.Tests.Handlers.Products
{
    public class BrowseProductsHandlerTests : BrowseHandlerTestBase<BrowseProducts, IProductRepository, Product, ProductDto>
    {
        protected override IQueryHandler<BrowseProducts, IPagedResult<ProductDto>> InstantiateHandler()
            => new BrowseProductsHandler(Repository, Mapper);
    }
}
