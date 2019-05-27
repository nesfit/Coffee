namespace Barista.Contracts.Commands.AuthenticationMeans
{
    public interface IResolveAuthenticationMeans : ICommand
    {
        string Method { get; }
        string Value { get; }
    }
}
