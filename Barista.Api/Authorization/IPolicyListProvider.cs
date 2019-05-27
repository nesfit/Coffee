using System.Collections.Generic;

namespace Barista.Api.Authorization
{
    public interface IPolicyListProvider
    {
        IEnumerable<KeyValuePair<string, string>> GetAll();
    }
}