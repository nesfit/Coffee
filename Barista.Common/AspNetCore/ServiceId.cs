using System;

namespace Barista.Common.AspNetCore
{
    public class ServiceId : IServiceId
    {
        private static readonly string UniqueId = $"{Guid.NewGuid():N}";
        public string Id => UniqueId;
    }
}
