using System;
using System.Collections.Generic;

namespace Diffstore.Serialization.Implementation
{
    public static class SupportedTypes
    {
        // TODO: Find a better way
        public static readonly IReadOnlyCollection<Type> List = new List<Type>()
        {
            typeof(byte)
        }.AsReadOnly();
    }
}