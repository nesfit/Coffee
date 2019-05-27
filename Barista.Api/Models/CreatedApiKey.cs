using System;

namespace Barista.Api.Models
{
    public class CreatedApiKey
    {
        public CreatedApiKey(Guid id, string value)
        {
            Id = id;
            Value = value;
        }

        public Guid Id { get; }
        public string Value { get; }
    }
}
