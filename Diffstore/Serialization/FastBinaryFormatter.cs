using System;
using System.Collections;
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

        private static Dictionary<Type, MethodInvoker> readMethods =
            new Dictionary<Type, MethodInvoker>();
        private static Dictionary<Type, MethodInvoker> writeMethods =
            new Dictionary<Type, MethodInvoker>();

        static FastBinaryFormatter()
        {
            var allReaderMethods = typeof(BinaryReader).GetMethods();
            Array.ForEach(allReaderMethods, (method) =>
            {
                if (!method.Name.StartsWith("Read")) return;
                if (method.Name.Length == 4) return;
                if (method.Parameters().Count != 0) return;
                var returnType = method.ReturnType;
                readMethods.Add(returnType, method.DelegateForCallMethod());
            });

            var allWriterMethods = typeof(BinaryWriter).GetMethods();
            Array.ForEach(allWriterMethods, (method) =>
            {
                if (!method.Name.Equals("Write")) return;
                if (method.Parameters().Count != 1) return;
                var paramType = method.GetParameters()[0].ParameterType;
                writeMethods.Add(paramType, method.DelegateForCallMethod());
            });
        }

        public object Deserialize(Type type, BinaryReader stream, string fieldName = null)
        {
            if (stream.BaseStream.CanSeek)
                if (stream.BaseStream.Length == stream.BaseStream.Position)
                    return null;

            bool isNotNull = stream.ReadBoolean();
            if (!isNotNull) return null;

            switch (type)
            {
                case Type t when t.IsArray:
                    return DeserializeArray(t, stream);
                case Type t when t.IsGenericList():
                    return DeserializeList(type, stream);
                case Type t when t.IsGenericDictionary():
                    return DeserializeDictionary(type, stream);
                default:
                    CheckIfCanRead(type);
                    return readMethods[type](stream);
            }
        }

        private void Read(BinaryReader stream, int count, Type itemType,
            Action<object> setter)
        {
            if (count == 0) return;
            if (stream.BaseStream.CanSeek)
                if (stream.BaseStream.Length == stream.BaseStream.Position)
                    return;
#pragma warning disable CS0168
            try
            {
                for (int i = 0; i < count; i++)
                    setter(readMethods[itemType](stream));
            }
            catch (EndOfStreamException ex) { }
#pragma warning restore CS0168
        }

        private object DeserializeArray(Type type, BinaryReader stream)
        {
            var itemType = type.GetElementType();
            var count = stream.ReadInt32();
            CheckIfCanRead(itemType);
            var instance = Array.CreateInstance(itemType, count);
            int i = 0;
            Read(stream, count, itemType, v => instance.SetElement(i++, v));
            return instance;
        }

        private object DeserializeList(Type type, BinaryReader stream)
        {
            var itemType = type.GenericTypeArguments[0];
            CheckIfCanRead(itemType);
            var instance = ConstructorCache.Create(typeof(List<>).MakeGenericType(itemType))
                as IList;

            var count = stream.ReadInt32();
            Read(stream, count, itemType, v => instance.Add(v));
            return instance;
        }

        private object DeserializeDictionary(Type type, BinaryReader stream)
        {
            var keyType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];
            CheckIfCanRead(keyType);
            CheckIfCanRead(valueType);
            var instance = ConstructorCache.Create(typeof(Dictionary<,>)
                .MakeGenericType(keyType, valueType)) as IDictionary;

            if (stream.BaseStream.CanSeek)
                if (stream.BaseStream.Length == stream.BaseStream.Position)
                    return instance;
#pragma warning disable CS0168
            try
            {
                int count = stream.ReadInt32();
                if (count == 0) return instance;

                for (int i = 0; i < count; i++)
                    instance.Add(readMethods[keyType](stream), readMethods[valueType](stream));
            }
            catch (EndOfStreamException ex) { }
#pragma warning restore CS0168
            return instance;
        }

        public void Serialize(object value, BinaryWriter stream, string fieldName = null)
        {
            switch (value)
            {
                case null:
                    stream.Write(false);
                    break;
                case IList list:
                    SerializeList(list, stream);
                    break;
                case IDictionary dictionary:
                    SerializeDictionary(dictionary, stream);
                    break;
                default:
                    var type = value.GetType();
                    CheckIfCanWrite(type);
                    stream.Write(true);
                    writeMethods[type](stream, value);
                    break;
            }
        }

        private void SerializeList(IList list, BinaryWriter stream)
        {
            stream.Write(true);
            stream.Write(list.Count);

            if (list.Count == 0) return;
            var type = list.GetType();
            var itemType = type.IsArray ? type.GetElementType() :
                type.GenericTypeArguments[0];

            foreach (var item in list)
                writeMethods[itemType](stream, item);
        }

        private void SerializeDictionary(IDictionary dict, BinaryWriter stream)
        {
            stream.Write(true);
            stream.Write(dict.Count);

            if (dict.Count == 0) return;
            var keyType = dict.GetType().GenericTypeArguments[0];
            var itemType = dict.GetType().GenericTypeArguments[1];
            CheckIfCanWrite(keyType);
            CheckIfCanWrite(itemType);

            foreach (var pair in dict.ZipPairs())
            {
                writeMethods[keyType](stream, pair.Item1);
                writeMethods[itemType](stream, pair.Item2);
            }
        }

        private void CheckIfCanRead(Type type)
        {
            if (!readMethods.ContainsKey(type))
                throw new ArgumentException($"No deserializer for type ${type}");
        }

        private void CheckIfCanWrite(Type type)
        {
            if (!writeMethods.ContainsKey(type))
                throw new ArgumentException($"No serializer for type ${type}");
        }
    }
}