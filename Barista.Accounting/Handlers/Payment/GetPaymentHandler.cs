using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Accounting.Dto;
using Barista.Accounting.Queries;
using Barista.Accounting.Repositories;
using Barista.Common;

namespace Barista.Accounting.Handlers.Payment
{
    public class GetPaymentHandler : IQueryHandler<GetPayment, PaymentDto>
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IMapper _mapper;

        public GetPaymentHandler(IPaymentsRepository paymentsRepository, IMapper mapper)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PaymentDto> HandleAsync(GetPayment query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return _mapper.MapToWithNullPropagation<PaymentDto>(await _paymentsRepository.GetAsync(query.Id));
        }
    }
}
