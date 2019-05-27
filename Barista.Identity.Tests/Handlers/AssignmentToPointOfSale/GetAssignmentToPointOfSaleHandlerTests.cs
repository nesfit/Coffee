using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Barista.Common.Tests;
using Barista.Identity.Dto;
using Barista.Identity.Handlers.AssignmentToPointOfSale;
using Barista.Identity.Queries;
using Barista.Identity.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barista.Identity.Tests.Handlers.AssignmentToPointOfSale
{
    [TestClass]
    public class GetAssignmentToPointOfSaleHandlerTests : GetHandlerTestBase<GetAssignmentToPointOfSaleHandler, GetAssignmentToPointOfSale, IAssignmentsRepository, Domain.Assignment, AssignmentToPointOfSaleDto>
    {
        protected override GetAssignmentToPointOfSaleHandler InstantiateHandler(IAssignmentsRepository repo, IMapper mapper)
            => new GetAssignmentToPointOfSaleHandler(repo, mapper);

        protected override GetAssignmentToPointOfSale InstantiateQuery()
            => new GetAssignmentToPointOfSale(TestIds.A);

        protected override Domain.Assignment InstantiateDomain()
            => (Domain.AssignmentToPointOfSale) FormatterServices.GetSafeUninitializedObject(typeof(Domain.AssignmentToPointOfSale));

        protected override Func<GetAssignmentToPointOfSale, Expression<Func<IAssignmentsRepository, Task<Domain.Assignment>>>> RepositorySetup
            => query => repo => repo.GetAsync(query.Id);
    }
}
