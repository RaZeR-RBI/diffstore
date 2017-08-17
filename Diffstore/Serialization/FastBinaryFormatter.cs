using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Diffstore.Utils;
using Fasterflect;

namespace Diffstore.Serialization
{
    /// <summary>
    /// Custom binary formatter based on BinaryReader/BinaryWriter for improved performance
    /// </summary>
    public class FastBinaryFormatter : IFormatter<BinaryReader, BinaryWriter>
    {
        private static FastBinaryFormatter _instance = new FastBinaryFormatter();
        public static FastBinaryFormatter Instance
        {
            get { return _instance; }
        }

        private static Dictionary<Type, MethodInfo> readMethods = new Dictionary<Type, MethodInfo>();
        private static Dictionary<Type, MethodInfo> writeMethods = new Dictionary<Type, MethodInfo>();

        static FastBinaryFormatter()
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

        public object Deserialize(Type type, BinaryReader stream, string fieldName = null)
        {
            if (!readMethods.ContainsKey(type))
                throw new ArgumentException($"No deserializer for type ${type}");

            bool isNotNull = stream.ReadBoolean();
            if (!isNotNull) return null;
            return readMethods[type].Call(stream);
        }

        public void Serialize(object value, BinaryWriter stream, string fieldName = null)
        {
            if (value == null) {
                stream.Write(false); return;
            }

            var type = value.GetType();
            if (!writeMethods.ContainsKey(type))
                throw new ArgumentException($"No serializer for type ${type}");

            stream.Write(true);
            writeMethods[type].Call(stream, value);
        }
    }
}