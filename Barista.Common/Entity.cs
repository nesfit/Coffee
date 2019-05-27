using System;

namespace Barista.Common
{
    public class Entity : IIdentifiable
    {
        public Guid Id { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset Updated { get; private set; }

        public Entity(Guid id) : this(id, DateTime.UtcNow, DateTime.UtcNow)
        {
        }

        protected Entity(Guid id, DateTimeOffset created, DateTimeOffset updated)
        {
            Id = id;
            Created = created;
            Updated = updated;
        }

        protected void SetUpdatedNow()
        {
            Updated = DateTimeOffset.UtcNow;
        }
    }
}
