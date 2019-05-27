using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.PointsOfSale.Services;

namespace Barista.PointsOfSale.Verifiers
{
    public class AccountingGroupVerifier : ExistenceVerifierBase<Guid>, IAccountingGroupVerifier
    {
        private readonly IAccountingGroupsService _service;

        public AccountingGroupVerifier(IAccountingGroupsService service)
            : base("accounting_group", "accounting group")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatAccountingGroup(id);
    }
}
