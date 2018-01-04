using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Diffstore.Entities;

namespace Diffstore.Serialization
{
    internal static class SchemaManager
    {
        private static Dictionary<Type, Schema> schemas = new Dictionary<Type, Schema>();

        public static void Register(Type type)
        {
            lock (schemas) schemas.Add(type, new Schema(type));
        }

        public static Schema Get(Type type)
        {
            lock (schemas)
            {
                if (!schemas.ContainsKey(type)) Register(type);
                return schemas[type];
            }
        }

        public static Schema Get<T>()
        {
            return Get(typeof(T));
        }
    }
}