using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.PointsOfSale.Dto;
using Barista.PointsOfSale.Queries;
using Barista.PointsOfSale.Repositories;

namespace Barista.PointsOfSale.Handlers.PointOfSale
{
    public class BrowsePointsOfSaleHandler : IQueryHandler<BrowsePointsOfSale, IPagedResult<PointOfSaleDto>>
    {
        private readonly IPointOfSaleRepository _pointsOfSaleRepository;
        private readonly IMapper _mapper;

        public BrowsePointsOfSaleHandler(IPointOfSaleRepository pointsOfSaleRepository, IMapper mapper)
        {
            _pointsOfSaleRepository = pointsOfSaleRepository ?? throw new ArgumentNullException(nameof(pointsOfSaleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<PointOfSaleDto>> HandleAsync(BrowsePointsOfSale query)
        {
            var results = await _pointsOfSaleRepository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<PointOfSaleDto>>(results);
        }
    }
}
