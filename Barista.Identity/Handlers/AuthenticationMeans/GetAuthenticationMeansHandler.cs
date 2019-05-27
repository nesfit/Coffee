using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class GetAuthenticationMeansHandler : IQueryHandler<GetAuthenticationMeans, AuthenticationMeansDto>
    {
        private readonly IAuthenticationMeansRepository _repository;
        private readonly IMapper _mapper;

        public GetAuthenticationMeansHandler(IAuthenticationMeansRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AuthenticationMeansDto> HandleAsync(GetAuthenticationMeans query)
        {
            return _mapper.MapToWithNullPropagation<AuthenticationMeansDto>(await _repository.GetAsync(query.Id));
        }
    }
}
