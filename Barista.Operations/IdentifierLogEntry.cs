using System;

namespace Barista.Operations
{
    public class IdentifierLogEntry
    {
        public IdentifierLogEntry(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
