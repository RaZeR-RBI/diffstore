using System;
using System.IO;
using Diffstore.Serialization;
using Diffstore.Serialization.XML;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class XmlFormatterTest : IFormatterTest
    {
        IFormatter<XmlDocumentAdapter, XmlWriterAdapter> formatter
            => XmlFormatter.Instance;

        XmlDocumentAdapter reader;
        XmlWriterAdapter writer;

        [Fact]
        public override void Test() => base.Test();

        internal override void BeginWrite(Stream input) =>
            writer = new XmlWriterAdapter(input);

        internal override void EndWrite() => writer.Dispose();

        internal override void BeginRead(Stream input) =>
            reader = new XmlDocumentAdapter(input);

        internal override void EndRead() => reader.Dispose();

        internal override void Serialize(object val, string fieldName) =>
            formatter.Serialize(val, writer, fieldName);

        internal override object Deserialize(Type type, string fieldName) =>
            formatter.Deserialize(type, reader, fieldName);
    }
}