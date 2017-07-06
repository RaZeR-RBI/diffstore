using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Diffstore.Serialization.Implementation;
using Diffstore.Utils;

namespace Diffstore.Serialization
{
    public static class Serializer
    {
        private static Dictionary<Type, object> collection = new Dictionary<Type, object>();

        static Serializer()
        {
            var allTypes = typeof(Deserializer).Assembly.GetTypes();
            foreach (var dataType in SupportedTypes.List)
            {
                var type = typeof(ITypeSerializer<>).MakeGenericType(dataType);
                var impl = allTypes.GetImplementingTypes(type).FirstOrDefault();
                var instance = Activator.CreateInstance(impl);
                collection.Add(dataType, instance);
            }
        }

        public static ITypeSerializer<T> For<T>()
        {
            var type = typeof(T);
            if (!collection.ContainsKey(type))
                throw new ArgumentException("No serializer for type " + type);
            return (ITypeSerializer<T>)collection[type];
        }
    }
}