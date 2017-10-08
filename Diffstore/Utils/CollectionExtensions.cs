using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diffstore.Utils
{
    public static class CollectionExtensions
    {
        public static IEnumerable<(object, object)> ZipPairs(this IDictionary dictionary)
        {
            var keys = dictionary.Keys.Cast<object>();
            var values = dictionary.Values.Cast<object>();
            return keys.Zip(values, (k, v) => (k, v));
        }
    }
}