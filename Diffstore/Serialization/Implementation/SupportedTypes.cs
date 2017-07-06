using System;
using System.Collections.Generic;

namespace Diffstore.Serialization.Implementation
{
    public static class SupportedTypes
    {
        // TODO: Find a better way
#if !CLS
        public static readonly IReadOnlyCollection<Type> List = new List<Type>()
        {
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
        }.AsReadOnly();

#else
        public static readonly IReadOnlyCollection<Type> List = new List<Type>()
        {
            typeof(byte),
            typeof(short),
            typeof(int),
        }.AsReadOnly();
#endif
    }
}