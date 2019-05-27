using System;

namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface IUpdateAuthenticationMeansValue : ICommand
    {
        Guid Id { get; }
        string Value { get; }
    }
}
