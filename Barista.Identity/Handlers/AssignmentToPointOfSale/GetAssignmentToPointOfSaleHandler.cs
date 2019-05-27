using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;

namespace Barista.Identity.Handlers.AssignmentToPointOfSale
{
    public class GetAssignmentToPointOfSaleHandler : IQueryHandler<GetAssignmentToPointOfSale, AssignmentToPointOfSaleDto>
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentsRepository _repository;

        public GetAssignmentToPointOfSaleHandler(IAssignmentsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AssignmentToPointOfSaleDto> HandleAsync(GetAssignmentToPointOfSale query)
        {
            return _mapper.MapToWithNullPropagation<AssignmentToPointOfSaleDto>((await _repository.GetAsync(query.Id)) as Domain.AssignmentToPointOfSale);
        }
    }
}
