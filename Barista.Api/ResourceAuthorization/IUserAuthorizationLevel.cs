namespace Barista.Api.ResourceAuthorization
{
    public interface IUserAuthorizationLevel
    {
        string Level { get; }
        int CompareTo(IUserAuthorizationLevel userAuthorizationLevel);
    }
}
