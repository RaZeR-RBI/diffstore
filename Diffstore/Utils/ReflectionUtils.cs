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
    }
}