using System;
using System.IO;
using System.Xml;

namespace Diffstore.Serialization.XML
{
    internal class XmlWriterAdapter : IDisposable
    {
        private XmlWriter _writer;
        public XmlWriter Instance
        {
            get => _writer;
        }

        public XmlWriterAdapter(Stream stream) {
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            _writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                CloseOutput = true,
                ConformanceLevel = ConformanceLevel.Document,
                WriteEndDocumentOnClose = true,
            });
            _writer.WriteStartDocument();
            _writer.WriteStartElement("Value");
        }

        public void Dispose() => _writer.Close();
    }
}