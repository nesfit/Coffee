using System;

namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface IDeleteAuthenticationMeans : ICommand
    {
        Guid Id { get; }
    }
}
