using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Diffstore.Serialization.Implementation;
using Diffstore.Utils;

namespace Diffstore.Serialization
{
    public static class Deserializer
    {
        private static Dictionary<Type, object> collection = new Dictionary<Type, object>();

        static Deserializer()
        {
            var allTypes = typeof(Deserializer).Assembly.GetTypes();
            foreach (var dataType in SupportedTypes.List)
            {
                var type = typeof(ITypeDeserializer<>).MakeGenericType(dataType);
                var impl = allTypes.GetImplementingTypes(type).FirstOrDefault();
                var instance = Activator.CreateInstance(impl);
                collection.Add(dataType, instance);
            }
        }

        public static ITypeDeserializer<T> For<T>()
        {
            var type = typeof(T);
            if (!collection.ContainsKey(type))
                throw new ArgumentException("No deserializer for type " + type);
            return (ITypeDeserializer<T>)collection[type];
        }
    }
}