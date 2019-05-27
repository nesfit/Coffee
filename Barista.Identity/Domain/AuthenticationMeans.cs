using System;
using Barista.Common;

namespace Barista.Identity.Domain
{
    public class AuthenticationMeans : Entity
    {
        public const int MaxValueLength = 1024;

        public AuthenticationMeans(Guid id, string label, string method, string value, DateTimeOffset validSince, DateTimeOffset? validUntil) : base(id)
        {
            SetMethod(method);
            SetValue(value);
            SetLabel(label);
            SetValidity(validSince, validUntil);
        }

        public string Method { get; private set; }
        public string Value { get; private set; }
        public string Label { get; private set; }
        public DateTimeOffset ValidSince { get; private set; }
        public DateTimeOffset? ValidUntil { get; private set; }

        public void SetMethod(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new BaristaException("invalid_method", "Method cannot be empty.");

            Method = method;
            SetUpdatedNow();
        }

        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BaristaException("invalid_value", "Value cannot be empty.");

            Value = value;
            SetUpdatedNow();
        }

        public void SetValidity(DateTimeOffset validSince, DateTimeOffset? validUntil)
        {
            if (validUntil != null && validSince > validUntil)
                throw new BaristaException("invalid_validity", $"{nameof(ValidSince)} must be less recent than ${nameof(ValidUntil)}");

            ValidSince = validSince;
            ValidUntil = validUntil;
            SetUpdatedNow();
        }

        public void SetLabel(string label)
        {
            Label = label;
            SetUpdatedNow();
        }
    }
}
