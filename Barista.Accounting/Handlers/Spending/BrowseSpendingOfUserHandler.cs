using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Handlers.Spending
{
    public class BrowseSpendingOfUserHandler : IQueryHandler<BrowseSpendingOfUser, IPagedResult<SpendingOfUserDto>>
    {
        private readonly ISpendingRepository _repository;
        private readonly IMapper _mapper;

        public BrowseSpendingOfUserHandler(ISpendingRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<SpendingOfUserDto>> HandleAsync(BrowseSpendingOfUser query)
        {
            return _mapper.Map<IPagedResult<SpendingOfUserDto>>(await _repository.BrowseAsync(query));
        }
    }
}
