using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class GetPointOfSaleHandler : IQueryHandler<GetPointOfSale, PointOfSaleDto>
    {
        private readonly IPointOfSaleRepository _repository;
        private readonly IMapper _mapper;

        public GetPointOfSaleHandler(IPointOfSaleRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PointOfSaleDto> HandleAsync(GetPointOfSale query)
        {
            var pos = await _repository.GetAsync(query.Id);
            return _mapper.MapToWithNullPropagation<PointOfSaleDto>(pos);
        }
    }
}
