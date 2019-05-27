using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Api.Commands.AuthenticationMeans
{
    public class DeleteAuthenticationMeans : IDeleteAuthenticationMeans
    {
        public DeleteAuthenticationMeans(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
