using System;
using System.Threading.Tasks;
using Barista.Common;
using Barista.SaleStrategies.Services;

namespace Barista.SaleStrategies.Domain
{
    public abstract class SaleStrategy : Entity
    {
        private static readonly DateTime CreatedAt = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public string DisplayName { get; }

        protected SaleStrategy(Guid id, string displayName) : base(id, CreatedAt, CreatedAt)
        {
            if (string.IsNullOrWhiteSpace(nameof(displayName)))
                throw new BaristaException("invalid_display_name", "Display name cannot be empty");

            DisplayName = displayName;
        }

        public abstract Task<bool> ApplyAsync(IAccountingService accountingService, Guid userId, decimal cost);
    }
}
