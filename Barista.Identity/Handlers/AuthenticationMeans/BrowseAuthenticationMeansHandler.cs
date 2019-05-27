using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    public class BrowseAuthenticationMeansHandler : IQueryHandler<BrowseAuthenticationMeans, IPagedResult<AuthenticationMeansDto>>
    {
        private readonly IAuthenticationMeansRepository _repository;
        private readonly IMapper _mapper;

        public BrowseAuthenticationMeansHandler(IAuthenticationMeansRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<AuthenticationMeansDto>> HandleAsync(BrowseAuthenticationMeans query)
        {
            return _mapper.Map<IPagedResult<AuthenticationMeansDto>>(await _repository.BrowseAsync(query));
        }
    }
}
