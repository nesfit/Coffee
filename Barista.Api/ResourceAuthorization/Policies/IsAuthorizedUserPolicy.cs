namespace Barista.Api.ResourceAuthorization.Policies
{
    public class IsAuthorizedUserPolicy : UserAuthorizationLevelPolicyBase
    {
        public static readonly IsAuthorizedUserPolicy Instance = new IsAuthorizedUserPolicy();

        public IsAuthorizedUserPolicy() : base("AuthorizedUser", "AuthorizedUser", "Owner")
        {
        }
    }
}
