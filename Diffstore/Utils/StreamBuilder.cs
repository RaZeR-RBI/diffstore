using System;
using System.Collections.Generic;
using System.IO;
using Fasterflect;
using Diffstore.Serialization;

namespace Diffstore.Utils
{
    internal static class StreamBuilder
    {
        private static Dictionary<Type, ConstructorInvoker> _streamCtorCache =
            new Dictionary<Type, ConstructorInvoker>();

        public static T FromStream<T>(Stream stream) => (T)FromStream(stream, typeof(T));

        public static object FromStream(Stream stream, Type type)
        {
            lock (_streamCtorCache)
            {
                if (!_streamCtorCache.ContainsKey(type))
                    _streamCtorCache.Add(type, type.DelegateForCreateInstance(typeof(Stream)));

                return _streamCtorCache[type].Invoke(stream);
            }
        }
    }
}