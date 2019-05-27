using System;
using Barista.Contracts;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class GetAuthenticationMeans : IQuery<AuthenticationMeansDto>
    {
        public GetAuthenticationMeans(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
