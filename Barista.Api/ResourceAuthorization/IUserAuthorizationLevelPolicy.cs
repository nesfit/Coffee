namespace Barista.Api.ResourceAuthorization.Loaders
{
    public interface IUserAuthorizationLevelPolicy
    {
        string RequiredLevel { get; }
        bool IsSatisfied(string userAuthorizationLevel);
    }
}
