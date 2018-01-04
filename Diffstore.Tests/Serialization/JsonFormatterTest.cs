using System;
using System.IO;
using Diffstore.Serialization;
using Diffstore.Serialization.JSON;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class JsonFormatterTest : IFormatterTest
    {
        IFormatter<JsonReaderAdapter, JsonWriterAdapter> formatter
            => JsonFormatter.Instance;

        JsonReaderAdapter reader;
        JsonWriterAdapter writer;

        [Fact]
        public override void Test() => base.Test();

        internal override void BeginWrite(Stream input) =>
            writer = new JsonWriterAdapter(input);

        internal override void EndWrite() => writer.Dispose();

        internal override void BeginRead(Stream input) =>
            reader = new JsonReaderAdapter(input);

        internal override void EndRead() => reader.Dispose();

        internal override void Serialize(object val, string fieldName) =>
            formatter.Serialize(val, writer, fieldName);

        internal override object Deserialize(Type type, string fieldName) =>
            formatter.Deserialize(type, reader, fieldName);
    }
}