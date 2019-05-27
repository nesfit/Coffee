using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Products.Dto;
using Barista.Products.Queries;
using Barista.Products.Repositories;

namespace Barista.Products.Handlers.Product
{
    public class GetProductHandler : IQueryHandler<GetProduct, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductDto> HandleAsync(GetProduct query)
        {
            var product = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<ProductDto>(product);
        }
    }
}
