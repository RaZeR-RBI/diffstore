using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diffstore.Utils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetImplementingTypes(this Type[] types, Type interfaceType)
        {
            return types.Where(type => type.GetInterfaces().Contains(interfaceType));
        }

        public static bool IsGenericList(this Type type) =>
            type.GetTypeInfo().IsGenericType && 
            (type.GetGenericTypeDefinition() == typeof(IList<>) ||
            type.GetGenericTypeDefinition() == typeof(List<>));

        public static bool IsGenericDictionary(this Type type) =>
            type.GetTypeInfo().IsGenericType && 
            (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
            type.GetGenericTypeDefinition() == typeof(Dictionary<,>));
    }
}