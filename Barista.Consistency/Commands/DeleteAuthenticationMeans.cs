using System;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Consistency.Commands
{
    public class DeleteAuthenticationMeans : IDeleteAuthenticationMeans
    {
        public Guid Id { get; set; }
    }
}
