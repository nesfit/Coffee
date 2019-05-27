using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Offers.Dto;
using Barista.Offers.Queries;
using Barista.Offers.Repositories;

namespace Barista.Offers.Handlers.Offer
{
    public class BrowseOffersHandler : IQueryHandler<BrowseOffers, IPagedResult<OfferDto>>
    {
        private readonly IOfferRepository _repository;
        private readonly IMapper _mapper;

        public BrowseOffersHandler(IOfferRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<OfferDto>> HandleAsync(BrowseOffers query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var offers = await _repository.BrowseAsync(query);
            return _mapper.Map<IPagedResult<OfferDto>>(offers);
        }
    }
}
