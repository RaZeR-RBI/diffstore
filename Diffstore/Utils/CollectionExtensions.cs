using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Defines helper functionality.
/// </summary>
namespace Diffstore.Utils
{
    /// <summary>
    /// Defines collection-related extension methods.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Transforms non-generic key-value pairs to tuples.
        /// </summary>
        public static IEnumerable<(object, object)> ZipPairs(this IDictionary dictionary)
        {
            var keys = dictionary.Keys.Cast<object>();
            var values = dictionary.Values.Cast<object>();
            return keys.Zip(values, (k, v) => (k, v));
        }
    }
}