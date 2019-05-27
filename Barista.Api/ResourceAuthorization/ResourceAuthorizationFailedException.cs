using System;

namespace Barista.Api.ResourceAuthorization
{
    public class ResourceAuthorizationFailedException : ApplicationException
    {
        public string ResourceName { get; }
        public object ResourceIdentifier { get; }
        public string RequiredLevel { get; }

        public ResourceAuthorizationFailedException() : base("Unauthorized access to resource")
        {

        }

        public ResourceAuthorizationFailedException(string resourceName, string requiredLevel, object resourceIdentifier)
        : this()
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(resourceName));
            if (string.IsNullOrEmpty(requiredLevel))
                throw new ArgumentException("Value cannot be null or empty.", nameof(requiredLevel));
            if (resourceIdentifier == null)
                throw new ArgumentNullException(nameof(resourceIdentifier));

            ResourceName = resourceName;
            RequiredLevel = requiredLevel;
            ResourceIdentifier = resourceIdentifier ?? throw new ArgumentNullException(nameof(resourceIdentifier));
        }
    }
}
