using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;
using Barista.Contracts;

namespace Barista.Accounting.Handlers.Payment
{
    public class BrowsePaymentsHandler : IQueryHandler<BrowsePayments, IPagedResult<PaymentDto>>
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IMapper _mapper;

        public BrowsePaymentsHandler(IPaymentsRepository paymentsRepository, IMapper mapper)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedResult<PaymentDto>> HandleAsync(BrowsePayments query)
            => _mapper.Map<IPagedResult<PaymentDto>>(await _paymentsRepository.BrowseAsync(query));
    }
}
