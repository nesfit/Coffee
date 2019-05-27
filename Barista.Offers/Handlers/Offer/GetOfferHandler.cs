using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Offers.Dto;
using Barista.Offers.Queries;
using Barista.Offers.Repositories;

namespace Barista.Offers.Handlers.Offer
{
    public class GetOfferHandler : IQueryHandler<GetOffer, OfferDto>
    {
        private readonly IOfferRepository _repository;
        private readonly IMapper _mapper;

        public GetOfferHandler(IOfferRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OfferDto> HandleAsync(GetOffer query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return _mapper.MapToWithNullPropagation<OfferDto>(await _repository.GetAsync(query.Id));
        }
    }
}
