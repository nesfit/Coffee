using System;
using System.Net.Http;
using System.Threading.Tasks;
using Barista.Common;
using Barista.StockOperations.Services;

namespace Barista.StockOperations.Verifiers
{
    public class UserVerifier : ExistenceVerifierBase<Guid>, IUserVerifier
    {
        private readonly IUsersService _service;

        public UserVerifier(IUsersService service)
            : base("user", "user")
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        protected override async Task<HttpResponseMessage> MakeRequest(Guid id)
            => await _service.StatUser(id);
    }
}
