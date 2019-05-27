using System;

namespace Barista.Common.EfCore
{
    public class EntityAlreadyExistsException : ApplicationException
    {
        public EntityAlreadyExistsException(Exception innerException) : base(
            "The entity with this identifier already exists.", innerException)
        {

        }
    }
}
