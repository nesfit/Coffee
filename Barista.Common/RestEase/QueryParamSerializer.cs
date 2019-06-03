using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestEase;

namespace Barista.Common.RestEase
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class QueryParamSerializer : RequestQueryParamSerializer
    {
        public override IEnumerable<KeyValuePair<string, string>> SerializeQueryParam<T>(string name, T value, RequestQueryParamSerializerInfo info)
            => Serialize(name, value, info);

        public override IEnumerable<KeyValuePair<string, string>> SerializeQueryCollectionParam<T>(string name, IEnumerable<T> values, RequestQueryParamSerializerInfo info)
            => Serialize(name, values, info);

        private IEnumerable<KeyValuePair<string, string>> Serialize<T>(string name, T value, RequestQueryParamSerializerInfo info)
        {
            if (value == null)
                yield break;

            foreach (var prop in GetPropertiesDeepRecursive(value, name))
            {
                if (prop.Value != null)
                    yield return new KeyValuePair<string, string>(prop.Key, prop.Value);
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetPropertiesDeepRecursive(object obj, string name)
        {
            var dict = new Dictionary<string, object>();

            if (obj == null) { 
                yield return new KeyValuePair<string, string>(name, null);
                yield break;
            }

            if (obj is DateTime dt)
            {
                yield return new KeyValuePair<string, string>(name, dt.ToString("o"));
                yield break;
            }

            if (obj is Guid guid)
            {
                yield return new KeyValuePair<string, string>(name, guid.ToString("D"));
                yield break;
            }

            if (obj.GetType().IsValueType || obj is string)
            {
                yield return new KeyValuePair<string, string>(name, obj.ToString());
                yield break;
            }

            if (obj is IEnumerable collection)
            {
                foreach (var item in collection)
                foreach (var subItem in GetPropertiesDeepRecursive(item, $"{name}")) // TODO: use the ?prop[]=x&prop[]=y format once the connected binding issues are figured out
                    yield return subItem;

                yield break;
            }

            var properties = obj.GetType().GetProperties();
            //If the prefix won't be empty, then it is needed to specify [Query(null)].
            //Otherwise, the query string will contain the query name e.g. 'query.page' instead of just 'page'. 
            //var prefix = string.IsNullOrWhiteSpace(name) ? string.Empty : $"{name}.";
            var prefix = string.Empty;
            foreach (var prop in properties)
            foreach (var subProp in GetPropertiesDeepRecursive(prop.GetValue(obj, null), $"{prop.Name}"))
                yield return subProp;
        }
    }
}
