using System;
using Barista.Common;
using Microsoft.EntityFrameworkCore.Internal;

namespace Barista.Accounting.Domain
{
    public class Payment : Entity
    {
        public decimal Amount { get; private set; }
        public Guid UserId { get; private set; }
        public string Source { get; private set; }
        public string ExternalId { get; private set; }

        public Payment(Guid id, decimal amount, Guid userId, string source, string externalId) : base(id)
        {
            SetAmount(amount);
            SetUserId(userId);
            SetSource(source);
            SetExternalId(externalId);
        }

        public Payment(Guid id, decimal amount, Guid userId, string source, string externalId, DateTimeOffset created) : base(id, created, DateTimeOffset.UtcNow)
        {
            SetAmount(amount);
            SetUserId(userId);
            SetSource(source);
            SetExternalId(externalId);
        }

        public void SetAmount(decimal amount)
        {
            if (amount <= decimal.Zero)
                throw new BaristaException("invalid_amount", "Amount must be a positive number");

            Amount = amount;
            SetUpdatedNow();
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
            SetUpdatedNow();
        }

        public void SetSource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new BaristaException("invalid_source", "Source cannot be empty");

            Source = source;
            SetUpdatedNow();
        }

        public void SetExternalId(string externalId)
        {
            ExternalId = externalId;
            SetUpdatedNow();
        }
    }
}
