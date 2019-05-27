using System;
using Barista.Common;

namespace Barista.Identity.Domain
{
    public abstract class Assignment : Entity
    {
        protected Assignment(Guid id, Guid means, DateTimeOffset validSince, DateTimeOffset? validUntil) : base(id)
        {
            SetMeansId(means);
            SetValidity(validSince, validUntil);
        }

        public Guid MeansId { get; protected set; }
        public DateTimeOffset ValidSince { get; protected set; }
        public DateTimeOffset? ValidUntil { get; protected set; }

        public void SetMeansId(Guid meansId)
        {
            MeansId = meansId;
            SetUpdatedNow();
        }

        public void SetValidity(DateTimeOffset validSince, DateTimeOffset? validUntil)
        {
            if (validUntil != null && validUntil < validSince)
                throw new BaristaException("invalid_validity", $"{nameof(ValidSince)} must be less recent than {nameof(ValidUntil)}");

            ValidSince = validSince;
            ValidUntil = validUntil;
            SetUpdatedNow();
        }
    }
}
