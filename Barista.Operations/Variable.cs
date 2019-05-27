using System.Collections;
using System.Collections.Generic;

namespace Barista.Operations
{
    public class Variable : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public Variable(string name, object value)
        {
            _values.Add(name, value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _values).GetEnumerator();
        }
    }
}
