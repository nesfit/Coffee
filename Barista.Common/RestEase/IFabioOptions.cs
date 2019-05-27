using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Common.RestEase
{
    public interface IFabioOptions
    {
        string Url { get; set; }
        int RequestRetries { get; set; }
    }
}
