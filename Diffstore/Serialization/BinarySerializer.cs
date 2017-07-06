using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Diffstore.Utils;
using Fasterflect;

namespace Diffstore.Serialization
{
    public class BinarySerializer : ISerializer<BinaryReader, BinaryWriter>
    {
        private static BinarySerializer _instance = new BinarySerializer();
        public static BinarySerializer Instance
        {
            get { return _instance; }
        }

        private static Dictionary<Type, MethodInfo> readMethods = new Dictionary<Type, MethodInfo>();
        private static Dictionary<Type, MethodInfo> writeMethods = new Dictionary<Type, MethodInfo>();

        static BinarySerializer()
        {
            var allReaderMethods = typeof(BinaryReader).GetMethods();
            Array.ForEach(allReaderMethods, (method) => {
                if (!method.Name.StartsWith("Read")) return;
                if (method.Name.Length == 4) return;
                if (method.Parameters().Count != 0) return;
                var returnType = method.ReturnType;
                readMethods.Add(returnType, method);
            });

            var allWriterMethods = typeof(BinaryWriter).GetMethods();
            Array.ForEach(allWriterMethods, (method) => {
                if (!method.Name.Equals("Write")) return;
                if (method.Parameters().Count != 1) return;
                var paramType = method.GetParameters()[0].ParameterType;
                writeMethods.Add(paramType, method);
            });
        }

        public object Deserialize(Type type, BinaryReader stream)
        {
            if (!readMethods.ContainsKey(type))
                throw new ArgumentException("No deserializer for type " + type);

            return readMethods[type].Call(stream);
        }

        public void Serialize(object value, BinaryWriter stream)
        {
            var type = value.GetType();
            if (!writeMethods.ContainsKey(type))
                throw new ArgumentException("No serializer for type " + type);

            writeMethods[type].Call(stream, value);
        }
    }
}