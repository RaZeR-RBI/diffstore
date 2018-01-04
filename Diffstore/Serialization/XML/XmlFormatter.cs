using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Diffstore.Utils;
using SharpFileSystem;

/// <summary>
/// XML format support
/// </summary>
namespace Diffstore.Serialization.XML
{
    internal class XmlFormatter : IFormatter<XmlDocumentAdapter, XmlWriterAdapter>
    {
        private static XmlFormatter _instance = new XmlFormatter();
        public static XmlFormatter Instance => _instance;

        private Dictionary<Type, Func<string, object>> _readers =
            new Dictionary<Type, Func<string, object>>() {
                { typeof(bool), s => bool.Parse(s) },
                { typeof(char), s => char.Parse(s) },
                { typeof(string), s => s },
                { typeof(byte), s => byte.Parse(s) },
                { typeof(short), s => short.Parse(s) },
                { typeof(int), s => int.Parse(s) },
                { typeof(long), s => long.Parse(s) },
                { typeof(float), s => float.Parse(s) },
                { typeof(double), s => double.Parse(s) },
                { typeof(decimal), s => decimal.Parse(s) },
#if !CLS
                { typeof(sbyte), s => sbyte.Parse(s) },
                { typeof(ushort), s => ushort.Parse(s) },
                { typeof(uint), s => uint.Parse(s) },
                { typeof(ulong), s => ulong.Parse(s) }
#endif
            };

        public object Deserialize(Type type, XmlDocumentAdapter stream, string fieldName = null)
        {
            var node = stream.Instance.SelectSingleNode("Value/" + fieldName);
            if (node == null) return null;

            switch (type)
            {
                case Type t when t.IsArray:
                    return DeserializeArray(type, node);
                case Type t when t.IsGenericList():
                    return DeserializeList(type, node);
                case Type t when t.IsGenericDictionary():
                    return DeserializeDictionary(type, node);
                default:
                    CheckIfSupported(type);
                    return DeserializeSingle(type, node.InnerText);
            }
        }

        private object DeserializeSingle(Type type, string value) => _readers[type](value);

        private void Read(XmlNode root, Action<string> setter)
        {
            if (!root.HasChildNodes) return;
            foreach(XmlNode node in root.ChildNodes)
                setter(node.InnerText);
        }

        private object DeserializeArray(Type type, XmlNode node)
        {
            var itemType = type.GetElementType();
            var instance = Array.CreateInstance(itemType, node.ChildNodes.Count);
            int i = 0;
            Read(node, s => instance.SetValue(DeserializeSingle(itemType, s), i++));
            return instance;
        }

        private object DeserializeList(Type type, XmlNode node)
        {
            var itemType = type.GenericTypeArguments[0];
            var instance = ConstructorCache.Create(typeof(List<>).MakeGenericType(itemType)) as IList;
            Read(node, s => instance.Add(DeserializeSingle(itemType, s)));
            return instance;
        }

        private object DeserializeDictionary(Type type, XmlNode node)
        {
            var keyType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];
            var instance = ConstructorCache.Create(typeof(Dictionary<,>)
                .MakeGenericType(keyType, valueType)) as IDictionary;

            if (!node.HasChildNodes) return instance;
            foreach (XmlNode child in node.ChildNodes)
            {
                var key = child.Attributes["key"].Value;
                var value = child.Attributes["value"].Value;
                instance.Add(DeserializeSingle(keyType, key),
                    DeserializeSingle(valueType, value));
            }

            return instance;
        }

        public void Dispose() { }

        public void Serialize(object value, XmlWriterAdapter writer, string fieldName = null)
        {
            if (value == null) return;
            var instance = writer.Instance;
            instance.WriteStartElement(fieldName);
            switch (value)
            {
                case null: break;
                case IList list: SerializeList(list, instance); break;
                case IDictionary dict: SerializeDictionary(dict, instance); break;
                default: instance.WriteString(value.ToString()); break;
            }
            instance.WriteEndElement();
        }

        private void SerializeList(IList list, XmlWriter writer)
        {
            foreach (var item in list)
            {
                writer.WriteStartElement("item");
                writer.WriteString(item.ToString());
                writer.WriteEndElement();
            }
        }

        private void SerializeDictionary(IDictionary dict, XmlWriter writer)
        {
            foreach (var pair in dict.ZipPairs())
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("key", pair.Item1.ToString());
                writer.WriteAttributeString("value", pair.Item2.ToString());
                writer.WriteEndElement();
            }
        }

        private void CheckIfSupported(Type type)
        {
            if (!_readers.ContainsKey(type))
                throw new ArgumentException($"Unsupported type ${type}");
        }
    }
}