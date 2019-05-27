using System;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Dto;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.Extensions.Logging;

namespace Barista.Identity.Handlers.AuthenticationMeans
{
    // TODO: unit tests
    public class BrowseMeansAssignedToPointOfSaleHandler : IQueryHandler<BrowseAssignedMeansToPointOfSale, IPagedResult<AuthenticationMeansWithValueDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAssignedMeansRepository _assignedMeansRepository;
        private readonly ILogger<BrowseMeansAssignedToPointOfSaleHandler> _logger;

        public BrowseMeansAssignedToPointOfSaleHandler(IAssignedMeansRepository assignedMeansRepository, IMapper mapper, ILogger<BrowseMeansAssignedToPointOfSaleHandler> logger)
        {
            _assignedMeansRepository = assignedMeansRepository ?? throw new ArgumentNullException(nameof(assignedMeansRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IPagedResult<AuthenticationMeansWithValueDto>> HandleAsync(BrowseAssignedMeansToPointOfSale query)
        {
            var resultsPage = await _assignedMeansRepository.BrowseMeansAssignedToPointOfSale(query.PointOfSaleId, query);
            return _mapper.Map<IPagedResult<AuthenticationMeansWithValueDto>>(resultsPage);
        }
    }
}
