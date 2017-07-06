using System;
using System.Collections.Generic;
using System.IO;
using Diffstore.Serialization;

namespace Diffstore.Tests.Serialization
{
    public static class TestUtil
    {
        public static void Setup<T>(
            out MemoryStream stream,
            out ITypeSerializer<T> serializer,
            out ITypeDeserializer<T> deserializer)
        {
            stream = new MemoryStream();
            serializer = Serializer.For<T>();
            deserializer = Deserializer.For<T>();
        }

        public static T ProcessValue<T>(
            T source,
            ITypeSerializer<T> serializer,
            ITypeDeserializer<T> deserializer,
            Stream stream) where T : IComparable<T>
        {
            serializer.Serialize(source, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return deserializer.Deserialize(stream);
        }
    }
}