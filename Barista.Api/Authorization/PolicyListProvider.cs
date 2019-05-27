using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Barista.Api.Authorization
{
    public class PolicyListProvider : IPolicyListProvider
    {
        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            return typeof(Policies).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .ToDictionary(fi => fi.Name, fi => (string)fi.GetRawConstantValue());
        }
    }
}
