using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.Balance
{
    public class GetBalanceHandler : IQueryHandler<GetBalance, BalanceDto>
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IMapper _mapper;

        public GetBalanceHandler(IBalanceRepository balanceRepository, IMapper mapper)
        {
            _balanceRepository = balanceRepository ?? throw new ArgumentNullException(nameof(balanceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BalanceDto> HandleAsync(GetBalance query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return _mapper.MapToWithNullPropagation<BalanceDto>(await _balanceRepository.GetAsync(query.UserId));
        }
    }
}
