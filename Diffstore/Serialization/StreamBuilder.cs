using System;
using System.Collections.Generic;
using System.IO;
using Fasterflect;
using Diffstore.Serialization;

namespace Diffstore.Serialization
{
    public static class StreamBuilder
    {
        private static Dictionary<Type, ConstructorInvoker> _streamCtorCache =
            new Dictionary<Type, ConstructorInvoker>();

        public static T FromStream<T>(Stream stream)
        {
            lock (_streamCtorCache)
            {
                var type = typeof(T);
                if (!_streamCtorCache.ContainsKey(type))
                    _streamCtorCache.Add(type, type.DelegateForCreateInstance(typeof(Stream)));

                return (T)_streamCtorCache[type].Invoke(stream);
            }
        }
    }
}