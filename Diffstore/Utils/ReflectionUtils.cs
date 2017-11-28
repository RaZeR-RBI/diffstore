using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diffstore.Utils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetImplementingTypes(this Type[] types,
            Type interfaceType) =>
            types.Where(type => type.GetInterfaces().Contains(interfaceType));

        public static bool IsGenericList(this Type type) =>
            type.GetTypeInfo().IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(IList<>) ||
            type.GetGenericTypeDefinition() == typeof(List<>));

        public static bool IsGenericDictionary(this Type type) =>
            type.GetTypeInfo().IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
            type.GetGenericTypeDefinition() == typeof(Dictionary<,>));

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}