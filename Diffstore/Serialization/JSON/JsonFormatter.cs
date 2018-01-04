using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Diffstore.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// JSON format support
/// </summary>
namespace Diffstore.Serialization.JSON
{
    internal class JsonFormatter : IFormatter<JsonReaderAdapter, JsonWriterAdapter>
    {
        private static JsonFormatter _instance = new JsonFormatter();
        public static JsonFormatter Instance => _instance;

        public object Deserialize(Type type, JsonReaderAdapter stream, string fieldName)
        {
            var obj = stream.Instance;
            var fieldValue = obj[fieldName];
            return fieldValue.ToObject(type);
        }

        public void Serialize(object value, JsonWriterAdapter stream, string fieldName)
        {
            var instance = stream.Instance;
            instance.WritePropertyName(fieldName);
            switch (value)
            {
                case null: instance.WriteNull(); break;
                case IList list: SerializeList(list, instance); break;
                case IDictionary dict: SerializeDictionary(dict, instance); break;
                default: instance.WriteValue(value); break;
            }
        }

        private void SerializeList(IList list, JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var item in list)
                writer.WriteValue(item);
            writer.WriteEndArray();
        }

        private void SerializeDictionary(IDictionary dict, JsonWriter writer)
        {
            writer.WriteStartObject();
            foreach(var pair in dict.ZipPairs())
            {
                writer.WritePropertyName(pair.Item1.ToString());
                writer.WriteValue(pair.Item2);
            }
            writer.WriteEndObject();
        }
    }
}