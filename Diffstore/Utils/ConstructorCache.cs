using System;
using System.Collections.Generic;
using Fasterflect;

namespace Diffstore.Utils
{
    /// <summary>
    /// Caches constructor delegates to speed up object creation
    /// </summary>
    public static class ConstructorCache
    {
        private static Dictionary<Type, ConstructorInvoker> _delegates =
            new Dictionary<Type, ConstructorInvoker>();

        /// <summary>
        /// Creates an object of type T using parameterless constructor
        /// </summary>
        /// <returns>New instance of T</returns>
        public static T Create<T>() => (T)Create(typeof(T));

        /// <summary>
        /// Creates an object of the specified type using parameterless constructor
        /// </summary>
        /// <returns>New instance of T</returns>
        public static object Create(Type type)
        {
            if (!_delegates.ContainsKey(type))
                _delegates.Add(type, type.DelegateForCreateInstance());

            return _delegates[type]();
        }
    }
}