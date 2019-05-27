namespace Barista.Api.ResourceAuthorization.Policies
{
    public class IsOwnerPolicy : UserAuthorizationLevelPolicyBase
    {
        public static readonly IsOwnerPolicy Instance = new IsOwnerPolicy();

        public IsOwnerPolicy() : base("Owner", "AuthorizedUser", "Owner")
        {
        }
    }
}
