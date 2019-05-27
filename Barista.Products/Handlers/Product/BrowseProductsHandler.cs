using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Products.Dto;
using Barista.Products.Queries;
using Barista.Products.Repositories;

namespace Barista.Products.Handlers.Product
{
    public class BrowseProductsHandler : IQueryHandler<BrowseProducts, IPagedResult<ProductDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public BrowseProductsHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<ProductDto>> HandleAsync(BrowseProducts query)
        {
            var products = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<ProductDto>>(products);
        }
    }
}
