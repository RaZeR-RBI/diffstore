using System;
using System.Collections.Generic;
using System.Reflection;
using Diffstore.Entity;

namespace Diffstore.Serialization
{
    public static class SchemaManager
    {
        private static Dictionary<Type, Schema> schemas = new Dictionary<Type, Schema>();

        public static void Register(Type type)
        {
            throw new NotImplementedException();
        }

        public static void Discover(Assembly assembly)
        {
            Discover(assembly, new[] { typeof(EntityAttribute) });
        }

        public static void Discover(Assembly assembly, params Type[] attributes)
        {
            throw new NotImplementedException();
        }

        public static Schema Get(Type type)
        {
            if (!schemas.ContainsKey(type))
                throw new ArgumentException($"No entity with type {type} registered");

            return schemas[type];
        }
    }
}