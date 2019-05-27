using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Barista.Api.Authorization
{
    public static class Policies
    {
        public const string IsUser = "Barista.Api.Authorization.Policies.IsUser";
        public const string IsPointOfSale = "Barista.Api.Authorization.Policies.IsPointOfSale";
        public const string IsAdvancedUser = "Barista.Api.Authorization.Policies.IsAdvancedUser";
        public const string IsAdministrator = "Barista.Api.Authorization.Policies.IsAdministrator";

        public const string CreateAccountingGroups = "Barista.Api.Authorization.Policies.CreateAccountingGroups";

        public const string CreatePayments = "Barista.Api.Authorization.Policies.CreatePayments";
        public const string DeletePayments = "Barista.Api.Authorization.Policies.DeletePayments";
        public const string UpdatePayments = "Barista.Api.Authorization.Policies.UpdatePayments";

        public const string CreateUsers = "Barista.Api.Authorization.Policies.CreateUsers";
        public const string UpdateUsers = "Barista.Api.Authorization.Policies.UpdateUsers";
        public const string DeleteUsers = "Barista.Api.Authorization.Policies.DeleteUsers";

        public const string CreatePointsOfSale = "Barista.Api.Authorization.Policies.CreatePoinsOfSale";

        public const string BrowseUserSummaries = "Barista.Api.Authorization.Policies.BrowseUserSummaries";
        public const string BrowseUserDetails = "Barista.Api.Authorization.Policies.BrowseUserDetails";

        public const string ViewUserSummary = "Barista.Api.Authorization.Policies.ViewUserSummary";
        public const string ViewUserDetails = "Barista.Api.Authorization.Policies.ViewUserDetails";

        public const string BrowseAssignmentsToPointOfSale = "Barista.Api.Authorization.Policies.BrowseAssignmentsToPointOfSale";
        public const string BrowseAssignmentsToUser = "Barista.Api.Authorization.Policies.BrowseAssignmentsToUser";

        public const string BrowseSpendingLimits = "Barista.Api.Authorization.Policies.BrowseSpendingLimits";
    }
}
