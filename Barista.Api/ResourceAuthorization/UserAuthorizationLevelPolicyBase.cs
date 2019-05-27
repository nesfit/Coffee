using System;
using System.Collections.Generic;
using Barista.Api.ResourceAuthorization.Loaders;

namespace Barista.Api.ResourceAuthorization
{
    public abstract class UserAuthorizationLevelPolicyBase : IUserAuthorizationLevelPolicy
    {
        protected readonly IList<string> ReferenceSortedList;
        protected readonly int RequiredLevelIndex;

        protected UserAuthorizationLevelPolicyBase(string requiredUserLevel, params string[] referenceSortedList)
        {
            if (string.IsNullOrWhiteSpace(requiredUserLevel))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(requiredUserLevel));

            ReferenceSortedList = referenceSortedList ?? throw new ArgumentNullException(nameof(referenceSortedList));
            RequiredLevelIndex = ReferenceSortedList.IndexOf(requiredUserLevel);
            RequiredLevel = requiredUserLevel;

            if (RequiredLevelIndex == -1)
                throw new ArgumentException("The required user level is not present in reference list", nameof(requiredUserLevel));
        }

        public string RequiredLevel { get; }

        public bool IsSatisfied(string userAuthorizationLevel)
        {
            if (string.IsNullOrWhiteSpace(userAuthorizationLevel))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(userAuthorizationLevel));

            return ReferenceSortedList.IndexOf(userAuthorizationLevel) >= RequiredLevelIndex;
        }
    }
}
