using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Common
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
